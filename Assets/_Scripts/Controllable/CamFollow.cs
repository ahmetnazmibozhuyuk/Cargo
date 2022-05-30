using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    public class CamFollow : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.instance.CamFollowTarget != null)
                transform.position = GameManager.instance.CamFollowTarget.transform.position;
        }
    }
}