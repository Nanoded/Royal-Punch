using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AttackBehaviour))]
public class ResetCharacter : MonoBehaviour
{
    [SerializeField] private GameObject _armature;
    private PlayerMovement _playerMovement;
    private MegaPunchController _megaPunchController;
    private MegaPunchAnimationEvents _megaPunchAnimationEvents;
    private AttackBehaviour _attackBehaviour;
    private PlayerRagdollManager _playerReactingToShockwave;
    private Animator _animator;
    private Vector3 _characterStartPosition;
    private Quaternion _characterStartRotation;

    void Start()
    {
        InitComponents();
        InitEvents();
    }

    private void InitComponents()
    {
        _characterStartPosition = transform.position;
        _animator = GetComponent<Animator>();
        _attackBehaviour = GetComponent<AttackBehaviour>();
        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
            _playerMovement.enabled = false;
        }
        if(TryGetComponent(out PlayerRagdollManager reaction))
        {
            _playerReactingToShockwave = reaction;
        }
        if(TryGetComponent(out MegaPunchController controller))
        {
            _megaPunchController = controller;
        }
        if(TryGetComponent(out MegaPunchAnimationEvents animationEvents))
        {
            _megaPunchAnimationEvents = animationEvents;
        }
    }
    private void InitEvents()
    {
        EventsController.StartEvent.AddListener(StartGame);
        EventsController.RestartWinEvent.AddListener(RestartWinLevel);
        EventsController.RestartLoseEvent.AddListener(RestartLoseLevel);
    }

    private void StartGame()
    {
        if(_playerMovement != null)
        {
            _characterStartRotation = transform.localRotation;
            _playerMovement.enabled = true;
            _animator.CrossFade("IdleInFight", 0.1f);
        }
        if(_playerReactingToShockwave != null)
        {
            _playerReactingToShockwave.enabled = true;
        }
        if(_megaPunchController != null)
        {
            _animator.enabled = true;
            _megaPunchController.StopAllCoroutines();
            _animator.CrossFade("Idle", 0.1f);
            StartCoroutine(_megaPunchController.PlayMegaPunch());
        }
        _attackBehaviour.enabled = true;
        _armature.SetActive(false);
    }

    private void ResetCharacters(string NameAnimationAfterReset)
    {
        _attackBehaviour.enabled = true;
        _armature.SetActive(false);
        if (_playerMovement != null)
        {
            _animator.enabled = true;
            transform.position = _characterStartPosition;
            transform.rotation = _characterStartRotation;
            _animator.CrossFade(NameAnimationAfterReset, 0.1f);
        }
        else
        {
            _megaPunchController.StopAllCoroutines();
            _megaPunchAnimationEvents.StopAllCoroutines();
            _animator.CrossFade("Idle", 0.1f);
        }
    }

    private void RestartWinLevel()
    {
        ResetCharacters("WinDance");

    }
    private void RestartLoseLevel()
    {
        ResetCharacters("Lose");
    }
}
