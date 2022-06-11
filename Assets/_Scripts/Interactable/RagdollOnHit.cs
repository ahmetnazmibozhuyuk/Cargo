using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class RagdollOnHit : MonoBehaviour
    {
        [SerializeField] private float hitForceX = 20;
        [SerializeField] private float hitForceY = 20;
        [SerializeField] private float hitForceZ = 20;

        [SerializeField] private GameObject ragdoll;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private IControlNPC _controlNPC;

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _controlNPC = GetComponent<IControlNPC>();
        }

        #region Collision Related Methods
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Truck")) return;
            GameManager.instance.MainTruck.HitSomething();          //trigger the truck hit method
            GetHit(collision.transform.position);
        }
        private void GetHit(Vector3 hittingObjectPosition)
        {
            _controlNPC.RemoveControl();
            _rigidbody.useGravity = true;
            _animator.enabled = false;
            ragdoll.SetActive(true);
            _rigidbody.AddForce(FlyForce(hittingObjectPosition));
            Debug.Log(_rigidbody.velocity);
            Debug.Log(FlyForce(hittingObjectPosition));
            Destroy(this);

        }
        private Vector3 FlyForce(Vector3 hittingObjectPosition)
        {
            Vector3 direction = (hittingObjectPosition - transform.position).normalized;
            return new Vector3(-direction.x * hitForceX, hitForceY, -direction.z * hitForceZ);
        }
        #endregion
    }
}
