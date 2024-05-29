using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AttackBehaviour))]
public class ResetCharacter : MonoBehaviour
{
    [SerializeField] private GameObject _armature;
    [SerializeField] private Collider[] _colliders;
    private PlayerMovement _playerMovement;
    private MegaPunchController _megaPunchController;
    private MegaPunchAnimationEvents _megaPunchAnimationEvents;
    private AttackBehaviour _attackBehaviour;
    private PlayerRagdollManager _playerReactingToShockwave;
    private Animator _animator;
    private Vector3 _characterStartPosition;
    private Quaternion _characterStartRotation;
    private bool _isStartScreen;

    void Start()
    {
        _isStartScreen = true;
        //foreach (Collider collider in _colliders)
        //{
        //    collider.enabled = false;
        //}
        _characterStartPosition = transform.position;
        _characterStartRotation = transform.localRotation;
        InitComponents();
        InitEvents();
    }

    private void Update()
    {
        if(_isStartScreen)
        {
            _attackBehaviour.Attack(false);
            transform.position = _characterStartPosition;
            transform.rotation = _characterStartRotation;
        }
    }

    private void InitComponents()
    {
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
        _isStartScreen = false;
        //foreach(Collider collider in _colliders)
        //{
        //    collider.enabled = true;
        //} 

        if(_playerMovement != null)
        {
            _playerMovement.enabled = true;
            _animator.CrossFade("IdleInFight", 0.1f);
            _animator.SetLayerWeight(1, 1);
            _animator.SetLayerWeight(2, 1);
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
        //foreach (Collider collider in _colliders)
        //{
        //    collider.enabled = false;
        //}
        _attackBehaviour.Attack(false);
        _armature.SetActive(false);
        if (_playerMovement != null)
        {
            _animator.enabled = true;
            _animator.CrossFade(NameAnimationAfterReset, 0.1f);
        }
        else
        {
            _megaPunchController.StopAllCoroutines();
            _megaPunchAnimationEvents.StopAllCoroutines();
            _animator.CrossFade("Idle", 0.1f);
        }
        transform.position = _characterStartPosition;
        transform.rotation = _characterStartRotation;
    }

    private void RestartWinLevel()
    {
        ResetCharacters("WinDance");

    }
    private void RestartLoseLevel()
    {
        ResetCharacters("Lose");
    }

    public void ReturnCharacter()
    {
        _isStartScreen = true;
        //transform.DOMove(_characterStartPosition, .01f);
        transform.position = _characterStartPosition;
        transform.rotation = _characterStartRotation;
    }
}
