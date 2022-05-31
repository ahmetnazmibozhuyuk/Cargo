using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    public class CamFollow : MonoBehaviour
    {
        [Tooltip("Higher the value is, lower the delay will be.")]
        [SerializeField] private float cameraPositionDelay = 4f;
        private void FixedUpdate()
        {
            if (GameManager.instance.CamFollowTarget != null)
                transform.position = CamPosition();
        }
        private Vector3 CamPosition()
        {
            return Vector3.Lerp(transform.position, GameManager.instance.CamFollowTarget.transform.position, Time.deltaTime * cameraPositionDelay);
        }
    }
}