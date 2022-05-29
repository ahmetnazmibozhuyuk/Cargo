using UnityEngine;

namespace Template.Control
{
    [RequireComponent(typeof(Rigidbody))]
    public class RunnerControl : MonoBehaviour
    {
        [SerializeField] private float maxControlSpeed;
        [SerializeField] private float maxForwardSpeed;
        [SerializeField] private float turnRate;

        [Tooltip("Determines the maximum movement touch displacement multiplier.")]
        [SerializeField] private float maxControlClamp;
        [Tooltip("Width of the plane the player moves.")]
        [SerializeField] private float horizontalClampLimit;

        private Rigidbody _rigidbody;

        private Vector3 _hitDownPosition;
        private Vector3 _offset;
        private Vector3 _offsetOnXZ;
        //private Vector3 _rotateVector;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
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
                _offset = Vector3.ClampMagnitude((Input.mousePosition - _hitDownPosition), maxControlClamp);
                _offsetOnXZ = new Vector3(_offset.x, _offset.z, _offset.y);

                if (transform.position.x >= horizontalClampLimit || transform.position.x <= -horizontalClampLimit)
                    _hitDownPosition = Input.mousePosition;

                //if (_offsetOnXZ != Vector3.zero)
                //    _rotateVector = _offsetOnXZ;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _offset = Vector3.zero;
                _offsetOnXZ = Vector3.zero;
            }
        }
        private void AssignMovement()
        {
            _rigidbody.MovePosition(new Vector3(Mathf.Clamp(transform.position.x + maxControlSpeed * Time.deltaTime * _offsetOnXZ.x, -horizontalClampLimit, horizontalClampLimit),
                transform.position.y - 0.05f + maxControlSpeed * Time.deltaTime * _offsetOnXZ.y,
                maxForwardSpeed + transform.position.z));


            transform.rotation = Quaternion.Euler(0, 0, transform.position.x * -50);
        }
        #endregion
    }
}