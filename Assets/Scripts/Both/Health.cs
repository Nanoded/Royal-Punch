using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    private Animator _animator;
    private PlayerMovement _playerMovement;

    public static UnityEvent EnemyDeathEvent = new UnityEvent();
    public static UnityEvent PlayerDeathEvent = new UnityEvent();

    void Start()
    {
        _currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
        }
        EventsController.RestartLoseEvent.AddListener(ResetHealth);
        EventsController.RestartWinEvent.AddListener(ResetHealth);
    }

    private void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _healthBar.fillAmount = _currentHealth / _maxHealth;
    }

    private void Death()
    {
        _animator.enabled = false;
        if(_playerMovement != null)
        {
            _playerMovement.enabled = false;
            PlayerDeathEvent.Invoke();
        }
        else
        {
            EnemyDeathEvent.Invoke();
        }
        StopAllCoroutines();
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.fillAmount = _currentHealth / _maxHealth;
        if(_currentHealth <= 0)
        {
            Death();
        }
    }
}
