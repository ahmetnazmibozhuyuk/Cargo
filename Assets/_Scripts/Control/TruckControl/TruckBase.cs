using UnityEngine;
using PathCreation;
using Cargo.Managers;
using Cargo.Interactable;

namespace Cargo.Control
{
    [RequireComponent(typeof(Stockpile))]
    public abstract class TruckBase : MonoBehaviour
    {
        public PathCreator pathCreator;

        [SerializeField] protected TruckData truckData;

        private Stockpile _truckCargoBed;

        private EndOfPathInstruction _endOfPathInstruction;

        private float _distanceTravelled;

        protected virtual void Awake()
        {
            _truckCargoBed = GetComponent<Stockpile>();
        }
        protected virtual void Start()
        {
            GameManager.instance.InitializeCargoCapacity(truckData.TruckCapacity);
            _truckCargoBed.InitializePositions();
        }

        protected virtual void Update()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState)
                SetPositionRotation();
        }
        private void SetPositionRotation()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += truckData.Speed * Time.deltaTime;
                transform.SetPositionAndRotation(pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction),
                    pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction));
            }

        }
    }
}
