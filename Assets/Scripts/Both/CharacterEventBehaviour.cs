using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AttackBehaviour))]
public class CharacterEventBehaviour : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private MegaPunchController _megaPunchController;
    private MegaPunchAnimationEvents _megaPunchAnimationEvents;
    private AttackBehaviour _attackBehaviour;
    private PlayerReactingToShockwave _playerReactingToShockwave;
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
        if(TryGetComponent(out PlayerReactingToShockwave reaction))
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
        Health.EnemyDeathEvent.AddListener(BehaviourAfterEnemyDeath);
        Health.PlayerDeathEvent.AddListener(BehaviourAfterPlayerDeath);
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
    }

    private void ResetCharacters()
    {
        _attackBehaviour.enabled = true;
        if (_playerMovement != null)
        {
            _animator.enabled = true;
            transform.position = _characterStartPosition;
            transform.rotation = _characterStartRotation;

        }
        else
        {
            _megaPunchController.StopAllCoroutines();
            _megaPunchAnimationEvents.StopAllCoroutines();
        }
    }

    private void RestartWinLevel()
    {
        ResetCharacters();
        if (_playerMovement != null)
        {
            _animator.CrossFade("WinDance", 0.1f);
        }

    }
    private void RestartLoseLevel()
    {
        ResetCharacters();
        if (_playerMovement != null)
        {
            _animator.CrossFade("Lose", 0.1f);
        }
    }

    private void StopFight()
    {
        _attackBehaviour.enabled = false;
        _animator.CrossFade("Idle", 0.1f);
        _animator.SetBool("Attack", false);
        
        if (_playerReactingToShockwave != null)
        {
            _playerReactingToShockwave.StopAllCoroutines();
            _playerReactingToShockwave.enabled = false;
        }
    }

    private void BehaviourAfterEnemyDeath()
    {
        StopFight();
        if(_playerMovement != null)
        {
            _playerMovement.enabled = false;
            _animator.CrossFade("Win", 0.1f);
        }
        if (_megaPunchController != null)
        {
            _animator.enabled = false;
        }
    }
    
    private void BehaviourAfterPlayerDeath()
    {
        StopFight();
        if (_playerMovement != null)
        {
            _playerMovement.enabled = false;
            _animator.CrossFade("Lose", 0.1f);
        }
    }
}
