using UnityEngine;
using Cargo.Interactable;
using Cargo.Managers;

namespace Cargo.Control
{
    [RequireComponent(typeof(Stockpile))]
    public class StarterTruck : TrackFollowerBase
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

    }
}
