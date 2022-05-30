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
            GameManager.instance.AssignTruck(gameObject,truckData.TruckCapacity);
            _truckCargoBed.InitializePositions();
        }

        protected virtual void FixedUpdate()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState)
                SetPositionRotation();
        }
        private void SetPositionRotation()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += truckData.Speed * Time.deltaTime;

                transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);

                transform.eulerAngles = SetRotationEuler();
            }

            //z+90
            //y-90
        }
        private Vector3 SetRotationEuler()
        {
            return new Vector3(pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).x,
                pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).y,
                pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).z+90);
        }
    }
}
