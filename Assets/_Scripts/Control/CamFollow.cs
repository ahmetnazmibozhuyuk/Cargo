using UnityEngine;
using Template.Managers;

namespace Template.Control
{
    public class CamFollow : MonoBehaviour
    {
        private void Update()
        {
            transform.position = GameManager.instance.Player.transform.position;
        }
    }
}