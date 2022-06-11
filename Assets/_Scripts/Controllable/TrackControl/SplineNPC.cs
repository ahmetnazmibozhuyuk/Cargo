using UnityEngine;

namespace Cargo.Control
{
    public class SplineNPC : TrackFollowerBase, IControlNPC
    {
        [SerializeField] private float speed = 1;
        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            _speed = speed + Random.Range(-1.5f, 1.5f);
            _rotationOffsetZ = 90;
            _endOfPathInstruction = PathCreation.EndOfPathInstruction.Loop;
        }
        public void RemoveControl()
        {
            TrackPathCreator = null;
            Destroy(this);
        }
    }
    public interface IControlNPC
    {
        public void RemoveControl();
    }
}
