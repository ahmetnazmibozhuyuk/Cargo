using UnityEngine;
using Cargo.Managers;

namespace Cargo.Control
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class TopDownControl : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 1f;
        [SerializeField] private float turnRate = 5f;
        [SerializeField] private float maxMagnitude = 10f;

        private Rigidbody _rigidbody;
        private Animator _animator;

        private Vector3 _hitDownPosition;
        private Vector3 _offset;
        private Vector3 _offsetOnXZ;
        private Vector3 _rotateVector;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (GameManager.instance.CurrentState == GameState.DriveState)
            {
                _animator.SetFloat("MovementSpeed", 0);
                _animator.SetBool("WorkIsFinised", true);
                Destroy(this);
                return;
            }
            SetControl();
        }
        private void FixedUpdate()
        {
            AssignMovement();
        }
        #region Movement Controls
        private void SetControl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hitDownPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _offset = Vector3.ClampMagnitude((Input.mousePosition - _hitDownPosition), maxMagnitude);
                _offsetOnXZ = new Vector3(_offset.x, _offset.z, _offset.y);


                if (_offsetOnXZ != Vector3.zero)
                    _rotateVector = _offsetOnXZ;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _offset = Vector3.zero;
                _offsetOnXZ = Vector3.zero;
            }
            _animator.SetFloat("MovementSpeed", _offset.magnitude / maxMagnitude); // todo : expensive operation can be removed to save a few frames
        }
        private void AssignMovement()
        {
            _rigidbody.MovePosition(transform.position + maxSpeed * Time.deltaTime * _offsetOnXZ);
            if (_rotateVector != Vector3.zero)
                _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_rotateVector), Time.deltaTime * turnRate * 100));
        }
        #endregion

    }
}