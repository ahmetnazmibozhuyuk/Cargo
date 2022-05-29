using UnityEngine;
using PathCreation;
using Cargo.Managers;

namespace Cargo.Control
{
    public class TruckControl : MonoBehaviour
    {
        public PathCreator pathCreator;

        [SerializeField] private float speed;
        [SerializeField] private float accelerationRate;
        [SerializeField] private float decelerationRate;

        private EndOfPathInstruction _endOfPathInstruction;

        private float _distanceTravelled;


        private void Update()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState)
                SetPositionRotation();
        }
        private void SetPositionRotation()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += speed * Time.deltaTime;
                transform.SetPositionAndRotation(pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction),
                    pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction));
            }

        }
    }
}
