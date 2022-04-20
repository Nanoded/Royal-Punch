using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _healthCanvas;
    [SerializeField] private Image _healthbar;
    [SerializeField] private TextMeshProUGUI _healthCount;
    [SerializeField] private float _trainedHealth;
    private float _maxHealth;
    private float _currentHealth;
    private Animator _animator;
    private PlayerMovement _playerMovement;

    public static UnityEvent EnemyDeathEvent = new UnityEvent();
    public static UnityEvent PlayerDeathEvent = new UnityEvent();

    void Start()
    {
        _animator = GetComponent<Animator>();

        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
        }

        InitHealth();

        _healthCanvas.SetActive(false);

        EventsController.RestartLoseEvent.AddListener(ResetHealthSettings);
        EventsController.RestartWinEvent.AddListener(ResetHealthSettings);
        EventsController.StartEvent.AddListener(ViewHealthBar);
        EventsController.TrainingEvent.AddListener(AddPlayerHealth);
        EventsController.RemoveProgressEvent.AddListener(InitHealth);
        EnemyDeathEvent.AddListener(AddEnemyHealth);
    }

    private void InitHealth()
    {
        if(_playerMovement != null)
        {
            if(PlayerPrefs.HasKey("PlayerHealth"))
            {
                _maxHealth = PlayerPrefs.GetFloat("PlayerHealth");
            }
            else
            {
                _maxHealth = 100;
                PlayerPrefs.SetFloat("PlayerHealth", _maxHealth);
            }
            PlayerPrefs.Save();
        }
        else
        {
            if(PlayerPrefs.HasKey("EnemyHealth"))
            {
                _maxHealth = PlayerPrefs.GetFloat("EnemyHealth");
            }
            else
            {
                _maxHealth = 100;
                PlayerPrefs.SetFloat("EnemyHealth", _maxHealth);
            }
            PlayerPrefs.Save();
        }

        _currentHealth = _maxHealth;
        _healthCount.text = _currentHealth.ToString();
        _healthbar.fillAmount = _currentHealth / _maxHealth;
    }

    private void ResetHealthSettings()
    {
        _healthCanvas.SetActive(false);
    }

    private void ViewHealthBar()
    {
        _healthCanvas.SetActive(true);
        InitHealth();
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
        _healthbar.fillAmount = _currentHealth / _maxHealth;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            Death();
        }
        _healthCount.text = _currentHealth.ToString();
    }

    private void AddEnemyHealth()
    {
        _maxHealth += Random.Range(10, 41);
        if (_playerMovement == null)
        {
            PlayerPrefs.SetFloat("EnemyHealth", _maxHealth);
            PlayerPrefs.Save();
        }
    }

    private void AddPlayerHealth()
    {
        if (_playerMovement != null)
        {
            _maxHealth += _trainedHealth;
            PlayerPrefs.SetFloat("PlayerHealth", _maxHealth);
            PlayerPrefs.Save();
        }
    }
}
