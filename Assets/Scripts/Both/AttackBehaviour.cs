using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private float _damage;
    private Animator _animator;
    private Health _enemyHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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

    public void DealDamage()
    {
        if (_enemyHealth != null)
        {
            _enemyHealth.GetDamage(_damage);
        }
    }
}
