using UnityEngine;
using Cargo.Managers;

namespace Cargo.Interactable
{
    public class EndGoal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Truck"))
                GameManager.instance.ChangeState(GameState.DeliverState);
        }
    }
}
