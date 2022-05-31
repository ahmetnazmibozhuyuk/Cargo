using UnityEngine;
using Cargo.Interactable;
using Cargo.Managers;
using DG.Tweening;

namespace Cargo.Control
{
    [RequireComponent(typeof(Stockpile))]
    public class BasicTruckControl : TrackFollowerBase
    {
        [SerializeField] private TruckData truckData;

        private Stockpile _truckCargoBed;


        private void Awake()
        {
            _truckCargoBed = GetComponent<Stockpile>();
        }
        private void Start()
        {
            GameManager.instance.AssignTruck(gameObject, truckData.TruckCapacity);
            _speed = truckData.Speed;
            _rotationOffsetZ = 90;
            _currentSpeedMultipier = 0;
            _truckCargoBed.InitializePositions();

        }
        private void Update()
        {
            switch (GameManager.instance.CurrentState)
            {
                case GameState.DriveState:
                    SetMovementMultiplier();
                    break;
                case GameState.DeliverState:
                    _currentSpeedMultipier = 1;
                    break;
                case GameState.GameWon:
                    _currentSpeedMultipier = 0;
                    break;
                case GameState.GameLost:
                    _currentSpeedMultipier = 0;
                    break;
            }
        }
        private void SetMovementMultiplier()
        {
            if (Input.GetMouseButton(0))
            {
                _currentSpeedMultipier = Mathf.Lerp(_currentSpeedMultipier, 1, truckData.AccelerationRate);
            }
            else
            {
                _currentSpeedMultipier = Mathf.Lerp(_currentSpeedMultipier, 0, truckData.DecelerationRate);
            }
        }
        public void HitSomething()
        {
            for(int i = 0; i < Random.Range(3,6); i++)
            {
                
                GameObject objectToLose = _truckCargoBed.GiveObject();
                if (objectToLose == null) return;
                objectToLose.transform.DOMove(new Vector3(
                    objectToLose.transform.position.x + Random.Range(2, 5),
                    objectToLose.transform.position.y + Random.Range(2,5),
                    objectToLose.transform.position.z+ Random.Range(2, 5)), 0.2f);

                objectToLose.transform.DOScale(0, 0.2f);
            }

            _currentSpeedMultipier = -0.5f;

        }

    }
}
