using UnityEngine;
using PathCreation;

namespace Cargo.Control
{
    public abstract class TrackFollowerBase : MonoBehaviour
    {
        public PathCreator TrackPathCreator;

        protected float _speed = 1f;
        protected float _currentSpeedMultipier = 1f;
        protected float _rotationOffsetX;
        protected float _rotationOffsetY;
        protected float _rotationOffsetZ;

        protected EndOfPathInstruction _endOfPathInstruction;

        private float _distanceTravelled;

        protected virtual void FixedUpdate()
        {
                SetPositionRotation();
        }
        private void SetPositionRotation()
        {
            if (TrackPathCreator != null)
            {
                _distanceTravelled += _speed * _currentSpeedMultipier * Time.deltaTime;
                transform.position = TrackPathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
                transform.eulerAngles = SetRotationEuler();
            }
        }
        private Vector3 SetRotationEuler()
        {
            return new Vector3(TrackPathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).x+ _rotationOffsetX,
                TrackPathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).y+ _rotationOffsetY,
                TrackPathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction).z+ _rotationOffsetZ);
        }
    }
}
