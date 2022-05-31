using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class NPC : TrackFollowerBase
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float hitForceX = 20;
        [SerializeField] private float hitForceY = 20;
        [SerializeField] private float hitForceZ = 20;

        [SerializeField] private GameObject ragdoll;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _speed = speed;
            _rotationOffsetZ = 90;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Truck")) return;
            GameManager.instance.MainTruck.HitSomething();
            TrackPathCreator = null;

            _rigidbody.AddForce(FlyForce(collision.gameObject.transform.position));

            _animator.enabled = false;
            ragdoll.SetActive(true);
            Destroy(this);
        }
        private Vector3 FlyForce(Vector3 hittingObjectPosition)
        {
            Vector3 direction = (hittingObjectPosition - transform.position).normalized;

            return new Vector3(-direction.x * hitForceX, hitForceY, -direction.z * hitForceZ);
        }
    }
}
