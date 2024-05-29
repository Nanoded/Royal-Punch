using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using YG;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _healthCanvas;
    [SerializeField] private Image _healthbar;
    [SerializeField] private TextMeshProUGUI _healthCount;
    [SerializeField] private int _trainedHealth;
    private int _maxHealth;
    private int _currentHealth;
    private Animator _animator;
    private PlayerMovement _playerMovement;

    public static UnityEvent EnemyDeathEvent = new UnityEvent();
    public static UnityEvent PlayerDeathEvent = new UnityEvent();

    public int CurrentHealth => _currentHealth;

    private void Awake()
    {
        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
        }
        YandexGame.GetDataEvent += InitHealth;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();

        InitHealth();

        EventsController.RestartLoseEvent.AddListener(HideHealthBar);
        EventsController.RestartWinEvent.AddListener(HideHealthBar);
        EventsController.StartEvent.AddListener(ViewHealthBar);
        EventsController.RemoveProgressEvent.AddListener(InitHealth);
        //EnemyDeathEvent.AddListener(AddEnemyHealth);
    }

    private void InitHealth()
    {
        if(_playerMovement != null)
        {
            LoadSaveHealthData(true);
        }
        else
        {
            LoadSaveHealthData(false);
        }

        _currentHealth = _maxHealth;
        _healthCount.text = _currentHealth.ToString();
        _healthbar.fillAmount = (float)((float)_currentHealth / (float)_maxHealth);
    }

    private void LoadSaveHealthData(bool isPlayer)
    {
        if (isPlayer)
        {
            _maxHealth = YandexGame.savesData.PlayerHealth;
        }
        else
        {
            _maxHealth = YandexGame.savesData.EnemyHealth;
        }
    }

    private void HideHealthBar()
    {
        InitHealth();
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
            if(EnemyDeathEvent != null)
                EnemyDeathEvent.Invoke();
            AddEnemyHealth();
        }
        StopAllCoroutines();
    }
    
    public void GetDamage(int damage)
    {
        if(_playerMovement == null)
        {
            _animator.CrossFade("GetDamage", 0.1f);
        }

        _currentHealth -= damage;
        _healthbar.fillAmount = (float)((float)_currentHealth / (float)_maxHealth);
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            Death();
        }
        _healthCount.text = _currentHealth.ToString();
    }

    private void AddEnemyHealth()
    {
        if (_playerMovement == null)
        {
            _maxHealth += Random.Range(20, 71);
            YandexGame.savesData.EnemyHealth = _maxHealth;
            YandexGame.SaveProgress();
        }
    }

    public void UpgradeHealth()
    {
        _maxHealth += _trainedHealth;
        _healthCount.text = _maxHealth.ToString();
        YandexGame.savesData.PlayerHealth = _maxHealth;
        YandexGame.SaveProgress();
    }
}
