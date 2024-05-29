using UnityEngine;
using YG;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private FixedJoystick _fixedJoystick;
    private Transform _enemy;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    public Vector3 AnimationMovementValue 
    { 
        get 
        { 
            if(YandexGame.EnvironmentData.isDesktop)
                return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            return _fixedJoystick.Direction;
        } 
        private set { } 
    }

    private void Awake()
    {
        _fixedJoystick.gameObject.SetActive(false);
        EventsController.StartEvent.AddListener(() =>
        {
            if (YandexGame.EnvironmentData.isMobile)
            {
                _fixedJoystick.gameObject.SetActive(true);
            }
        });
    }

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
        Vector3 forwardMovement = Vector3.zero;
        Vector3 rightMovement = Vector3.zero;

        if (YandexGame.EnvironmentData.isDesktop)
        {
            forwardMovement = transform.forward * Input.GetAxis("Vertical");
            rightMovement = transform.right * Input.GetAxis("Horizontal");
        }
        else
        {
            forwardMovement = transform.forward * _fixedJoystick.Direction.y;
            rightMovement = transform.right * _fixedJoystick.Direction.x;
        }
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
