using UnityEngine;

namespace Cargo.Control
{
    [CreateAssetMenu(fileName = "NewTruckData", menuName = "Data/TruckData")]
    public class TruckData : ScriptableObject
    {
        [Header("Truck Identity")]
        public string TruckName;
        public string TruckDescription;
        [Header("Truck Stats")]
        public float Speed;
        public float AccelerationRate;
        public float DecelerationRate;

        public int TruckCapacity;

        public int UnlockPrice;
    }
}
