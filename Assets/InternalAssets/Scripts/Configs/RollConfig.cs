using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "RollConfig")]
    public class RollConfig: ScriptableObject
    {
        public int cellsCount;
        
        [Header("Speed settings")]
        public float maxSpeed = 2000f;
        public float minSpeedBeforeStop = 30f;
        
        [Header("Duration")]
        public float accelerationDuration = 3f;
        public float decelerationDuration = 2f;
        public float centeringDuration = 1f;
        
        [Header("Blocks Spawning Behaviour")]
        public float spacing = 10f;
        public float cellCenteringY = 0f;
        public float cellDestroyingZone = -500f;
        public float startSpawnPositionY = 500f;
        
        [Header("Delays")]
        public float delayBetweenStartSpins = 1f;
        public float delayBetweenEndSpins = 1f;
        
        public float delayBeforeStopMachine = 5f;
        public float delayBeforeEnablingStop = 3f;
        public float baitDuration;
    }
}