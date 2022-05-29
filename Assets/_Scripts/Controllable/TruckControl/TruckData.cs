using UnityEngine;

namespace Cargo.Control
{
    [CreateAssetMenu(fileName = "NewTruckData", menuName = "TruckData")]
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
        [Header("Truck Price")]
        public int UnlockPrice;
    }
}
