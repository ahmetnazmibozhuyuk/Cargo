using UnityEngine;
using PathCreation;
using Cargo.Managers;


namespace Cargo.Control
{
    public abstract class TrackFollowerBase : MonoBehaviour
    {
        public PathCreator pathCreator;

        private EndOfPathInstruction _endOfPathInstruction;

        private float _distanceTravelled;

        protected float _speed = 1f;
        protected float _currentSpeedMultipier = 1f;

        protected float _rotationOffsetX;
        protected float _rotationOffsetY;
        protected float _rotationOffsetZ;
        

        protected virtual void FixedUpdate()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState || GameManager.instance.CurrentState == GameState.DeliverState)
                SetPositionRotation();
        }

        private void SetPositionRotation()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += _speed * _currentSpeedMultipier * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
                transform.eulerAngles = SetRotationEuler();
            }
        }
        private Vector3 SetRotationEuler()
        {
            return new Vector3(pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).x+ _rotationOffsetX,
                pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).y+ _rotationOffsetY,
                pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).z+ _rotationOffsetZ);
        }
    }
}
