using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using DefaultNamespace;
using UnityEngine;

public class LootboxController : MonoBehaviourExtBind
{
    [SerializeField] private List<DrumController> drums;
    [SerializeField] private RollConfig config;
    
    [SerializeField] Cell cellPrefab;

    private List<string> rewardList = new();
    
    [OnStart]
    public void InitDrums()
    {
        foreach (var drum in drums)
        {
            InstantiateDrumElements(drum);
            drum.MoveSequence(0);
        }
    }

    [Bind("OnRunSpinning")]
    public void StartSpinning()
    {
        Settings.Invoke("PlaySoundLoop", "running");
        ResetThis();
        rewardList.Clear();

        Debug.Log("StartSpinning");
        
        foreach (var drum in drums)
        {
            Path
                .Action(drum.StartSpin)
                .Wait(config.delayBetweenStartSpins);
        }

        Path
            .Wait(config.delayBeforeEnablingStop)
            .Action(() => Settings.Model.Set("CanBeStopped", true))
            .Wait(config.delayBeforeStopMachine - config.delayBeforeEnablingStop)
            .Action(() => Settings.Invoke("OnStopEvent"));
    }

    private void ResetThis()
    {
        Path = new CPath();
    }

    [Bind("StopSpinning")]
    public void StopSpinning()
    {
        ResetThis();
        
        Debug.Log("StopSpinning");
        Path
            .Action(drums[0].StopSpin);

    }

    //Срабатывает, если 2 барабана равны(для создания интриги)
    [Bind("OnBaitEvent")]
    public void OnBaitEventHandler()
    {
        Settings.Invoke("StopSoundLoop");
        Settings.Invoke("PlaySingleSound", $"drums_bait");
    }
    

    [Bind("OnSlotStopped")]
    public void OnSlotStopped(string id)
    {
        rewardList.Add(id);
        Settings.Invoke("PlaySingleSound", $"stop_{rewardList.Count}");
        
        if (rewardList.Count < drums.Count)
        {
            //Если был остановлен предпоследний барабан
            if (rewardList.Count == drums.Count - 1)
            {
                if (rewardList.All(x => x == rewardList[0]))
                {
                    ResetThis();
                    Settings.Invoke("OnBaitEvent");
                    Path
                        .Wait(config.baitDuration) // Ждем 4 секунды
                        .Action(() => 
                        {
                            var lastDrum = drums[rewardList.Count]; // Последний барабан
                            lastDrum.StopSpin();
                        });
                }
                else
                {
                    var lastDrum = drums[rewardList.Count];
                    lastDrum.StopSpin();
                }
            }
            else
            {
                var nextDrum = drums[rewardList.Count];
                nextDrum.StopSpin();
            }
        }
        // Проверка: если все барабаны остановились
        else if (rewardList.Count == drums.Count)
        {
            if (rewardList.All(x => x == rewardList[0]))
            {
                Settings.Invoke("ClaimPrizeEvent", rewardList);
            }

            // Вызываем событие остановки машины
            Settings.Invoke("OnMachineStopped");
            Settings.Invoke("StopSoundLoop");
        }
    }

    public void InstantiateDrumElements(DrumController drum)
    {
        List<string> itemsIds = Bootstrap.ItemDatabase.GetItemsIdsListByChance(config.cellsCount);

        for (int i = 0; i < config.cellsCount; i++)
        {
            var cellInstance = Instantiate(cellPrefab, drum.container);  // Создаем ячейку
            cellInstance.Item = Bootstrap.ItemDatabase.GetItemById(itemsIds[i]);

            if (drum.Head == null)
            {
                drum.Head = cellInstance;
            }
            else
            {
                drum.Tail.SetNext(cellInstance);  // Устанавливаем следующую ячейку
            }
            
            var pos = cellInstance.rectTransform.anchoredPosition;
            cellInstance.rectTransform.anchoredPosition = (pos + new Vector2(0, config.startSpawnPositionY)); 
            
            drum.Tail = cellInstance;  
        }
    }
}
