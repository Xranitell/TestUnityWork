using System;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;

namespace DefaultNamespace
{
    public class DrumController : MonoBehaviourExtBind
    {
        [SerializeField] public Transform container;
        private RectTransform _rectTransform;
        private float currentSpeed;
        
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = transform as RectTransform;
                return _rectTransform;
            }
        }
        RollConfig Config => Bootstrap.RollConfig;
        public Cell Head { get; set; }
        public Cell Tail { get; set; }
        public bool isSpinning { get; set; }

        public void StartSpin()
        {
            var config = Bootstrap.RollConfig;
            Path
                .Action(() => isSpinning = true)
                .EasingCubicEaseIn(config.accelerationDuration, 1, config.maxSpeed, (f) => currentSpeed = f)
                .Action(() => Settings.Invoke("OnAccelerationEnded"));
        }
        public void StopSpin()
        {
            Path
                .EasingQuadEaseOut(Config.decelerationDuration, currentSpeed, Config.minSpeedBeforeStop,
                    (curSpeed) => currentSpeed = curSpeed)
                .Action(() =>
                {
                    if (currentSpeed <= Config.minSpeedBeforeStop)
                    {
                        CenterOnTarget();
                        currentSpeed = Config.minSpeedBeforeStop; // Останавливаем колесо
                    }
                });

        }
        [OnUpdate]
        private void Loop()
        {
            if (isSpinning)
            {
                MoveSequence(currentSpeed);
            }
        }

        public void MoveSequence(float currentSpeed)
        {
            Tail.rectTransform.anchoredPosition -= new Vector2(0, currentSpeed * Time.deltaTime);
            var currentNode = Tail.Prev;

            while (currentNode != null)
            {
                var y = currentNode.Next.rectTransform.anchoredPosition.y + currentNode.rectTransform.rect.height + Config.spacing;

                currentNode.rectTransform.anchoredPosition = new Vector2(0, y);
                
                currentNode = currentNode.Prev;
            }
            if (Tail != null && Tail.rectTransform.anchoredPosition.y < Config.cellDestroyingZone)
            {
                MoveTailToTop();
            }
        }
        
        private void CenterOnTarget()
        {
            float closestDistance = float.MaxValue;
            Cell targetCell = null;

            Cell curCell = Head;
            while (curCell != null)
            {
                float distanceToTarget = Mathf.Abs(curCell.rectTransform.anchoredPosition.y - Config.cellCenteringY);
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    targetCell = curCell;
                }

                curCell = curCell.Next;
            }

            if (targetCell != null)
            {
                Vector3 cachedScale = targetCell.transform.localScale;
                float initialOffsetY = targetCell.rectTransform.anchoredPosition.y;
                float finalOffsetY = Config.cellCenteringY;

                Path = new CPath()
                    // Плавно доеезжаем до целевого элемента
                    .EasingQuadEaseOut(Config.centeringDuration, initialOffsetY, finalOffsetY, (newPositionY) =>
                    {
                        float offsetY = newPositionY - targetCell.rectTransform.anchoredPosition.y;

                        // Применяем смещение ко всем элементам относительно движения целевой ячейки
                        Cell current = Head;
                        while (current != null)
                        {
                            current.rectTransform.anchoredPosition += new Vector2(0, offsetY);
                            current = current.Next;
                        }
                    })
                    .Action(() =>
                    {
                        currentSpeed = 0f;
                        isSpinning = false;
                        Settings.Invoke("OnSlotStopped", targetCell.ItemId);
                    })
                    .EasingCubicEaseIn(0.1f, 1, 1.2f, (f) => targetCell.transform.localScale = cachedScale * f)
                    .EasingLinear(0.1f, 1.2f, 1, (f) => targetCell.transform.localScale = cachedScale * f);
            }
        }

        private void MoveTailToTop()
        {
            if (Tail == null || Head == null) return;

            Tail.rectTransform.anchoredPosition = new Vector2(
                Head.rectTransform.anchoredPosition.x,
                Head.rectTransform.anchoredPosition.y + Head.rectTransform.rect.height + Config.spacing
            );

            // Обновляем ссылки
            Cell newHead = Tail;
            Tail = Tail.Prev;

            if (Tail != null)
            {
                Tail.Next = null;
            }

            newHead.Prev = null;
            newHead.Next = Head;

            if (Head != null)
            {
                Head.Prev = newHead;
            }

            Head = newHead;
        }

        private void OnDrawGizmos()
        {
            var rect = RectTransform.rect;
            Gizmos.DrawWireCube(rect.center, rect.size);
        }
    }
}