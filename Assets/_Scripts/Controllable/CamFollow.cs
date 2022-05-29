using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    public class CamFollow : MonoBehaviour
    {
        private void Update()
        {
            transform.position = GameManager.instance.Player.transform.position;
        }
    }
}