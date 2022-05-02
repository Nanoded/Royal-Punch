using UnityEngine;

[RequireComponent(typeof(AttackBehaviour))]
public class DeathBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _armature;
    [SerializeField] private Rigidbody _hitBoneRigidbody;
    [SerializeField] private float _deathPunchForce;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private ParticleSystem[] _playerWinEffects;
    private PlayerMovement _playerMovement;
    private MegaPunchController _megaPunchController;
    private AttackBehaviour _attackBehaviour;
    private PlayerRagdollManager _playerReactingToShockwave;
    private Animator _animator;


    void Start()
    {
        InitComponents();
        EnemyDeathEffectStop();
        Health.EnemyDeathEvent.AddListener(BehaviourAfterEnemyDeath);
        Health.PlayerDeathEvent.AddListener(BehaviourAfterPlayerDeath);
        EventsController.RestartWinEvent.AddListener(EnemyDeathEffectStop);
    }

    private void InitComponents()
    {
        _animator = GetComponent<Animator>();
        _attackBehaviour = GetComponent<AttackBehaviour>();
        if (TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
            _playerMovement.enabled = false;
        }
        if (TryGetComponent(out PlayerRagdollManager reaction))
        {
            _playerReactingToShockwave = reaction;
        }
        if (TryGetComponent(out MegaPunchController controller))
        {
            _megaPunchController = controller;
        }
    }

    private void StopFight()
    {
        _attackBehaviour.enabled = false;
        _animator.CrossFade("Idle", 0.1f);
        _animator.SetBool("Attack", false);
        _hitBoneRigidbody.velocity = Vector3.zero;

        if (_playerReactingToShockwave != null)
        {
            _playerReactingToShockwave.StopAllCoroutines();
            _playerReactingToShockwave.enabled = false;
        }
    }

    private void BehaviourAfterEnemyDeath()
    {
        StopFight();
        if (_playerMovement != null)
        {
            _playerMovement.enabled = false;
            _animator.CrossFade("Win", 0.1f);
            EnemyDeathEffectPlay();
        }
        if (_megaPunchController != null)
        {
            _armature.SetActive(true);
            _animator.enabled = false;
            _megaPunchController.StopAllCoroutines();
            _hitBoneRigidbody.AddForce((transform.position - _enemy.transform.position) * _deathPunchForce, ForceMode.Impulse);
        }
    }

    private void BehaviourAfterPlayerDeath()
    {
        StopFight();
        if (_playerMovement != null)
        {
            _animator.enabled = false;
            _playerMovement.enabled = false;
            _animator.CrossFade("Lose", 0.1f);
            _hitBoneRigidbody.AddForce((_enemy.transform.position - transform.position) * _deathPunchForce, ForceMode.Impulse);
        }
        else if (_megaPunchController != null)
        {
            _megaPunchController.StopAllCoroutines();
        }
    }

    private void EnemyDeathEffectPlay()
    {
        foreach (var winEffect in _playerWinEffects)
        {
            winEffect.Play();
        }
    }
    
    private void EnemyDeathEffectStop()
    {
        foreach (var winEffect in _playerWinEffects)
        {
            winEffect.Stop();
        }
    }
}
