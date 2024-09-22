using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace InternalAssets
{
    [State("RunningState")]
    public class RunningState:FSMState
    {
        [Enter]
        public void Enter()
        {
            Debug.Log("RunningState");
            Settings.Invoke("OnRunSpinning");
            
            
            Settings.Model.Set("CanBeStarted", false);
            
        }

        [Bind("OnStopEvent")]
        public void StopSpin()
        {
            Parent.Change("CalculatingState");
        }
    }
}