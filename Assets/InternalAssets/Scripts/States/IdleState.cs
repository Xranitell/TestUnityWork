using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace InternalAssets
{
    [State("IdleState")]
    public class IdleState:FSMState
    {
        [Enter]
        public void Enter()
        {
            Debug.Log("Enter Idle state");
            
            Settings.Model.Set("CanBeStarted", true);
            Settings.Model.Set("CanBeStopped", false);
        }
        
        [Bind("OnPlayEvent")]
        public void OnSpinningStart()
        {
            Parent.Change("RunningState");
        }
        
    }
}