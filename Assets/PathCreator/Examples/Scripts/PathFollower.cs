using UnityEngine;

namespace PathCreation.Examples
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private float accelerationRate;
        [SerializeField] private float decelerationRate;
        public PathCreator pathCreator;
        private EndOfPathInstruction _endOfPathInstruction;

        private float _distanceTravelled;

        private void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        private void Update()
        {
            SetPositionRotation();
        }
        private void SetPositionRotation()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);

                transform.eulerAngles = pathCreator.path.GetRotationAtDistanceAsEuler(_distanceTravelled, _endOfPathInstruction);

            }

        }

        private void OnPathChanged()
        {
            _distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}