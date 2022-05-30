using UnityEngine;
using Cargo.Managers;

namespace Cargo
{
    public class EndGoal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GameManager.instance.ChangeState(GameState.DeliverState);
        }
    }
}
