using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private Transform _rightFist;
    [SerializeField] private Transform _leftFist;
    [SerializeField] private ParticleSystem _hitEffect;
    private Animator _animator;
    private Health _enemyHealth;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
        }
    }

    private void AttackLogic(Collider target, bool applyAttack)
    {
        if (target.TryGetComponent(out Health enemyHealth) )
        {
            _animator.SetBool("Attack", applyAttack);
            _enemyHealth = enemyHealth;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.enabled == true)
        {
            AttackLogic(other, true);
        }
        else
        {
            AttackLogic(other, false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        AttackLogic(other, false);
    }

    public void DealDamageLeftFist()
    {
        _hitEffect.transform.position = _leftFist.position;
        DealDamageLogic();
    }

    public void DealDamageRightFist()
    {
        _hitEffect.transform.position = _rightFist.position;
        DealDamageLogic();
    }

    private void DealDamageLogic()
    {
        if (_enemyHealth != null)
        {
            _enemyHealth.GetDamage(_damage);
        }
        if (_playerMovement != null)
        {
            _hitEffect.Play();
        }
    }
}
