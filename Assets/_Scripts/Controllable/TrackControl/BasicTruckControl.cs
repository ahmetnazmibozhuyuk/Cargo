using UnityEngine;
using Cargo.Interactable;
using Cargo.Managers;
using DG.Tweening;
using ObjectPooling;

namespace Cargo.Control
{
    [RequireComponent(typeof(Stockpile))]
    public class BasicTruckControl : TrackFollowerBase
    {
        [SerializeField] private TruckData truckData;

        [SerializeField] private int minHitLoss = 3;
        [SerializeField] private int maxHitLoss = 6;
        [SerializeField] private int minLossScatter = 3;
        [SerializeField] private int maxLossScatter = 10;

        [SerializeField] private float lossDuration = 0.6f;

        private Stockpile _truckCargoBed;

        private void Awake()
        {
            _truckCargoBed = GetComponent<Stockpile>();
        }
        private void Start()
        {
            Initialze();
        }
        private void Initialze()
        {
            GameManager.instance.AssignTruck(gameObject, truckData.TruckCapacity);
            _speed = truckData.Speed;
            _rotationOffsetZ = 90;
            _currentSpeedMultipier = 0;
            _truckCargoBed.InitializePositions();
        }
        protected override void FixedUpdate()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState || GameManager.instance.CurrentState == GameState.DeliverState)
                base.FixedUpdate();             // the truck is only active in spesific game states
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
        public void HitSomething() // is triggered from the NPC class
        {
            for(int i = 0; i < Random.Range(minHitLoss,maxHitLoss); i++)
            {
                GameObject objectToLose = _truckCargoBed.GiveObject();
                if (objectToLose == null) return;
                objectToLose.transform.DOMove(new Vector3(
                    objectToLose.transform.position.x + Random.Range(minLossScatter, maxLossScatter),
                    objectToLose.transform.position.y + Random.Range(minLossScatter, maxLossScatter),
                    objectToLose.transform.position.z+ Random.Range(minLossScatter, maxLossScatter)), lossDuration);

                objectToLose.transform.DOScale(0, lossDuration).OnComplete(() =>
                {
                    ObjectPool.Despawn(objectToLose); // send back the lost object back into the pool in its initial scale
                }
                );
            }
            _currentSpeedMultipier = -0.5f; // hit causes the truck to stop and go back momentarily; regains its speed from update even when there are no inputs
                                            // without input _currentSpeedMultiplier quickly goes back to zero so the truck won't keep moving backwards
        }
    }
}
