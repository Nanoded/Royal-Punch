using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Animator))]
public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private bool _isEnemey;

    [SerializeField] private GameObject _attackButton;

    [SerializeField] private Transform _rightFist;
    [SerializeField] private Transform _leftFist;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioSource _hitSound;

    [Header("Impact force")]
    [SerializeField] private int _damage;
    [SerializeField] private int _upgradeDamageCount;
    [SerializeField] private TextMeshProUGUI _forceText;

    [Header("Stamina")]
    [SerializeField] private Image _staminaImage;
    [SerializeField] private TextMeshProUGUI _staminaText;
    private int _maxStaminaCount = 6;
    private int _currentStaminaCount;
    private bool _startCoroutine;

    private Animator _animator;
    private Health _enemyHealth;
    private PlayerMovement _playerMovement;

    private bool _attack;

    private void Awake()
    {
        YandexGame.GetDataEvent += InitData;
    }

    private void Start()
    {
        if(!_isEnemey)
            _attackButton.SetActive(false);
        InitData();
        EventsController.StartEvent.AddListener(() =>
        {
            if(!_isEnemey)
            {
                if (YandexGame.EnvironmentData.isMobile)
                {
                    _attackButton.SetActive(true);
                }
                _staminaText.text = _maxStaminaCount.ToString();
                _currentStaminaCount = _maxStaminaCount;
            }
        });
        _animator = GetComponent<Animator>();
        if(TryGetComponent(out PlayerMovement playerMovement))
        {
            _playerMovement = playerMovement;
            
        }
        if(_isEnemey)
        {
            Health.EnemyDeathEvent.AddListener(UpgradeDamage);
        }
        else
        {
            EventsController.RestartLoseEvent.AddListener(() => _staminaText.text = _maxStaminaCount.ToString());
            EventsController.RestartWinEvent.AddListener(() => _staminaText.text = _maxStaminaCount.ToString());
        }
    }

    private void InitData()
    {
        if(_isEnemey)
        {
            _damage = YandexGame.savesData.EnemyForce;
        }
        else
        {
            _damage = YandexGame.savesData.PlayerForce;
            _maxStaminaCount = YandexGame.savesData.PlayerStamina;
            _staminaText.text = _maxStaminaCount.ToString();
            _forceText.text = _damage.ToString();
        }
    }

    private void Update()
    {
        if (_isEnemey) return;
        if (_currentStaminaCount > 0)
        {
            if(YandexGame.EnvironmentData.isDesktop)
            {
                if (Input.GetMouseButtonDown(0)) AttackLogic(true);
                else if (Input.GetMouseButtonUp(0)) AttackLogic(false);
            }
            else if(YandexGame.EnvironmentData.isMobile)
            {
                if (_attack)  AttackLogic(true);
                else if (!_attack) AttackLogic(false);
            }
        }
        else
        {
            AttackLogic(false);
        }
    }

    public void Attack(bool attack)
    {
        _attack = attack;
    }

    private IEnumerator RechargeStamina()
    {
        _startCoroutine = true;
        yield return new WaitForSeconds(2f);
        _currentStaminaCount++;
        _staminaImage.fillAmount = (float)((float)_currentStaminaCount / (float)_maxStaminaCount);
        _staminaText.text = _currentStaminaCount.ToString();
        if (_currentStaminaCount < _maxStaminaCount)
        {
            StartCoroutine(RechargeStamina());
        }
        else
            _startCoroutine = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (enabled == true)
        {
            if(other.TryGetComponent(out Health health))
            {
                _enemyHealth = health; 
                if(_isEnemey)
                {
                    AttackLogic(true);
                }
            }

        }
        else
        {
            if(_isEnemey)
                AttackLogic(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _enemyHealth = null;
        if(_isEnemey)
            AttackLogic(false);
    }

    public void AttackLogic(bool applyAttack)
    {
        _animator.SetBool("Attack", applyAttack);
    }

    public void TestDealDamageLogic(int isRightFist)
    {
        if(!_isEnemey)
        {
            _currentStaminaCount--;
            _staminaImage.fillAmount = (float)((float)_currentStaminaCount / (float)_maxStaminaCount);
            _staminaText.text = _currentStaminaCount.ToString();
            if(_startCoroutine == false)
            {
                StartCoroutine(RechargeStamina());
            }
        }

        if (_enemyHealth == null) return;

        if(_hitEffect != null)
        {
            if(isRightFist == 1)
                _hitEffect.transform.position = _rightFist.position;
            else
                _hitEffect.transform.position = _leftFist.position;
        }

        _hitSound.Play();
        _enemyHealth.GetDamage(_damage);

        if (_playerMovement != null)
        {
            _hitEffect.Play();
        }

        
    }

    public void UpgradeStamina()
    {
        _maxStaminaCount++;
        _staminaText.text = _maxStaminaCount.ToString();
        YandexGame.savesData.PlayerStamina = _maxStaminaCount;
        YandexGame.SaveProgress();
    }

    public void UpgradeDamage()
    {
        _damage += _upgradeDamageCount;
        if(!_isEnemey)
        {
            _forceText.text = _damage.ToString();
            YandexGame.savesData.PlayerForce = _damage;
        }
        else
        {
            YandexGame.savesData.EnemyForce = _damage;
        }
        YandexGame.SaveProgress();
    }
}
