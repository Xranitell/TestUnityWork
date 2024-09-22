using InternalAssets;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellView _cellView;
        [HideInInspector]public Cell Next;
        [HideInInspector]public Cell Prev;
        private RectTransform _rectTransform;

        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                _cellView.UpdateVisual(_item);
                UpdatePosition();
            }
        }
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = transform as RectTransform;
                return _rectTransform;
            }
        }
        public string ItemId => Item?.name;

        private void UpdatePosition()
        {
            if (Prev != null)
            {
                rectTransform.anchoredPosition = new Vector2(
                    Prev.rectTransform.anchoredPosition.x,
                    Prev.rectTransform.anchoredPosition.y - Prev.rectTransform.rect.height - Bootstrap.RollConfig.spacing // Получаем значение spacing от LootboxController
                );
            }
            else
            {
                rectTransform.anchoredPosition = Vector2.zero; // Первая ячейка
            }
        }

        public void SetNext(Cell next)
        {
            Next = next;
            if (Next != null)
            {
                Next.Prev = this;
                UpdatePosition();
            }
        }
    }


}