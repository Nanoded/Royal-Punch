using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private FloatingJoystick _floatingJoystick;
    private Transform _enemy;
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    public Vector3 AnimationMovementValue => _floatingJoystick.Direction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector3 forwardMovement = transform.forward * _floatingJoystick.Direction.y;
        Vector3 rightMovement = transform.right * _floatingJoystick.Direction.x;
        _direction = forwardMovement + rightMovement;
        _rigidbody.velocity = _direction * _speed;
    }

    private void Look()
    {
        if (_enemy != null)
        {
            transform.LookAt(_enemy);
        }
    }
}
