using UnityEngine;
using YG;

namespace NewScripts.Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _lookAt;
        private bool _isMobile;
        private Vector3 _direction;

        public Vector3 Direction => _direction;

        private void Start()
        {
            _isMobile = YandexGame.EnvironmentData.isMobile;
            //_joystick.gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(_lookAt);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {            
            if (_isMobile)
            {
                var forward = transform.forward * _joystick.Direction.y;
                var side = transform.right * _joystick.Direction.x;
                _direction = forward + side;
            }
            else
            {
                var forward = transform.forward * Input.GetAxisRaw("Vertical");
                var side = transform.right * Input.GetAxisRaw("Horizontal");
                _direction = forward + side;
            }
            _rigidbody.velocity = _direction.normalized * _speed * Time.fixedDeltaTime;
        }
    }
}
