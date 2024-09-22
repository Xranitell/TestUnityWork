using System.Collections;
using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using DefaultNamespace;
using InternalAssets;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviourExtBind
{
    public static ItemsDatabase ItemDatabase;
    public static RollConfig RollConfig;

    [SerializeField] private RollConfig rollConfig;
    [SerializeField] private ItemsDatabase itemsDatabase;
    
    [OnAwake]
    public void InitConfigs()
    {
        RollConfig = rollConfig;
        ItemDatabase = itemsDatabase;
    }

    
    [OnAwake]
    public void InitFms()
    {
        Settings.Fsm = new FSM();
        Settings.Fsm.Add(new IdleState());
        Settings.Fsm.Add(new RunningState());
        Settings.Fsm.Add(new CalculatingState());
        Settings.Fsm.Start("IdleState");
    }

    [Bind("OnStopButtonClick")]
    public void StopButtonClickHandler()
    {
        Settings.Invoke("OnStopEvent");
    }

    [Bind("OnPlayButtonClick")]
    public void PlayButtonClickHandler()
    {
        Settings.Invoke("OnPlayEvent");
    }
}
