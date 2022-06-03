using UnityEngine;

namespace Cargo.Control
{
    [CreateAssetMenu(fileName = "NewTruckData", menuName = "TruckData")]
    public class TruckData : ScriptableObject
    {
        [Header("Truck Identity")]
        public string TruckName;
        [Header("Truck Stats")]
        public float Speed;
        public float AccelerationRate;
        public float DecelerationRate;
        public int TruckCapacity;
        //public int MinLossOnCollision;
        //public int MaxLossOnCollision;
    }
}
