using System.Collections;
using UnityEngine;
using DG.Tweening;
using YG;

[RequireComponent(typeof(Animator))]
public class MegaPunchController : MonoBehaviour
{
    [SerializeField] private float _timeBeforeMegaPunch;
    [SerializeField] private float _timeChargingMegaPunch;
    [SerializeField] private float _timeMagnettoMode;
    [SerializeField] private float _megaPunchForce;
    [SerializeField] private float _magnetAtractionForce;
    [SerializeField] private int _megaPunchDamage;
    [SerializeField] private int _addDamage;
    [SerializeField] private float _distanceMegaPunch;
    [SerializeField] private AudioSource _punchSound;


    private bool _lookAtPlayer;
    private Vector3 _defaultScaleEffects;
    private Transform _player;
    private Rigidbody _playerRigidbody;
    private Animator _animator;
    private bool _isMagnet = false;

    public float DistanceMegaPunch => _distanceMegaPunch;

    private void Awake()
    {
        _megaPunchDamage = YandexGame.savesData.EnemyMegaPunchForce;
        YandexGame.GetDataEvent += () => _megaPunchDamage = YandexGame.savesData.EnemyMegaPunchForce;
        _lookAtPlayer = true;
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_player != null && _player.TryGetComponent(out Rigidbody playerRigidbody))
        {
            _playerRigidbody = playerRigidbody;
        }
        EventsController.RestartWinEvent.AddListener(() =>
        {
            _megaPunchDamage += _addDamage;
            YandexGame.savesData.EnemyMegaPunchForce = _megaPunchDamage;
            YandexGame.SaveProgress();
        });
    }

    private void Update()
    {
        LookAtPlayer(_lookAtPlayer);
        MagneticAtraction(_isMagnet);
    }


    private void LookAtPlayer(bool isLooking)
    {
        if (isLooking == true && _player != null)
        {
            transform.LookAt(_player);
        }
        else
        {
            transform.LookAt(transform.forward + transform.position);
        }
    }

    private void MagneticAtraction(bool isMagnet)
    {
        if (isMagnet == true)
        {
            _playerRigidbody.AddForce((_player.position + transform.position).normalized * _magnetAtractionForce, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement player))
        {
            if (_isMagnet == true)
            {
                _animator.CrossFade("MagnettoPunch", 0.1f);
                _isMagnet = false;
            }
        }
    }

    public IEnumerator MagnettoModeOn()
    {
        _isMagnet = true;
        yield return new WaitForSeconds(_timeMagnettoMode);
        _isMagnet = false;
        _animator.CrossFade("Relax", 0.1f);
    }

    public IEnumerator PlayMegaPunch()
    {
        _lookAtPlayer = true;
        yield return new WaitForSeconds(_timeBeforeMegaPunch);
        int punchID = Random.Range(0, 3);
        _animator.SetInteger("MegaPunchID", punchID);
        _animator.CrossFade("PreMegaPunch", 0.1f);
        StopAllCoroutines();
    }

    public IEnumerator ChargingMegaPunch(RectTransform effectAreaDamage, Vector3 scaleEffects)
    {

        _lookAtPlayer = false;
        effectAreaDamage.gameObject.SetActive(true);
        _defaultScaleEffects = effectAreaDamage.localScale;
        effectAreaDamage.DOScale(effectAreaDamage.localScale + scaleEffects, _timeChargingMegaPunch);
        yield return new WaitForSeconds(_timeChargingMegaPunch);
        _animator.StopPlayback();
    }

    public void ReloadPunchEffect(RectTransform megaPunchEffect)
    {
        megaPunchEffect.localScale = _defaultScaleEffects;
        megaPunchEffect.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void ShockWaveImpact(Collider target)
    {
        if (target.TryGetComponent(out PlayerRagdollManager playerReaction))
        {
            playerReaction.EnableRagdoll(_megaPunchForce, (playerReaction.transform.position * 2 + playerReaction.transform.up * 7) -  transform.position);
        }
        if (target.TryGetComponent(out Health health))
        {
            health.GetDamage(_megaPunchDamage);
            _punchSound.Play();
        }
    }
}
