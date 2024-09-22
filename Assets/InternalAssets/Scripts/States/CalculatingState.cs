using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace InternalAssets
{
    
    [State("CalculatingState")]
    public class CalculatingState:FSMState
    {
        [Enter]
        public void Enter()
        {
            //Останавливаю колесо и меняю состояние кнопки Stop
            Settings.Model.Set("CanBeStopped", false);
            Settings.Invoke("StopSpinning");
            
            Debug.Log("Start Calculating");
        }
        
        [Bind("OnMachineStopped")]
        public void OnMachineStopped()
        {
            Debug.Log("Machine Stopped");
            Parent.Change("IdleState");
        }
    }
}