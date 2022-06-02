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
            _speed = speed+Random.Range(-1.5f,1.5f);
            _rotationOffsetZ = 90;
            _endOfPathInstruction = PathCreation.EndOfPathInstruction.Loop;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Truck")) return;
            GameManager.instance.MainTruck.HitSomething();
            GetHit(collision.transform.position);
        }
        private void GetHit(Vector3 hittingObjectPosition)
        {
            TrackPathCreator = null;
            _rigidbody.useGravity = true;
            _animator.enabled = false;
            ragdoll.SetActive(true);
            _rigidbody.AddForce(FlyForce(hittingObjectPosition));
            Destroy(this);
        }
        private Vector3 FlyForce(Vector3 hittingObjectPosition)
        {
            Vector3 direction = (hittingObjectPosition - transform.position).normalized;
            return new Vector3(-direction.x * hitForceX, hitForceY, -direction.z * hitForceZ);
        }
    }
}
