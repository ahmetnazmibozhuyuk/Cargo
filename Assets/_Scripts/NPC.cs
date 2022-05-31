using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    [RequireComponent(typeof(Rigidbody))]
    public class NPC : TrackFollowerBase
    {
        [SerializeField] private float hitForceX = 20;
        [SerializeField] private float hitForceY = 20;
        [SerializeField] private float hitForceZ = 20;
        private Rigidbody _rigidbody;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _speed = 5;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Truck")) return;
            GameManager.instance.MainTruck.HitSomething();
            TrackPathCreator = null;
            _rigidbody.AddForce(FlyForce(collision.gameObject.transform.position));
            Destroy(this);
        }
        private Vector3 FlyForce(Vector3 hittingObjectPosition)
        {
            Vector3 direction = (hittingObjectPosition - transform.position).normalized;

            return new Vector3(-direction.x * hitForceX,  hitForceY, -direction.z * hitForceZ);
        }
    }
}
