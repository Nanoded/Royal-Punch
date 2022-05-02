using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MegaPunchController))]
[RequireComponent(typeof(Animator))]
public class MegaPunchAnimationEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem _circleShockWaveEffect;
    [SerializeField] private ParticleSystem _rectangleShockWaveEffect;
    [SerializeField] private ParticleSystem _magnettoEffect;
    [SerializeField] private ParticleSystem _chargeEffect;
    [SerializeField] private RectTransform _circleAreaDamage;
    [SerializeField] private RectTransform _rectangleAreaDamage;
    [SerializeField] private float _maxSizeScaleChargingEffects;
    [SerializeField] private float _timeToRelax;

    private Vector3 _scaleEffects;
    private Animator _animator;
    private MegaPunchController _megaPunchController;
    private Collider[] _hitColliders;

    private void Start()
    {
        Health.EnemyDeathEvent.AddListener(ResetEffects);
    }

    private void ResetEffects()
    {
        if (_circleAreaDamage.gameObject.activeInHierarchy)
        {
            _megaPunchController.ReloadPunchEffect(_circleAreaDamage);
        }
        else if(_rectangleAreaDamage.gameObject.activeInHierarchy)
        {
            _megaPunchController.ReloadPunchEffect(_rectangleAreaDamage);
        }
        _chargeEffect.Stop();
        _magnettoEffect.Stop();
    }

    private void Awake()
    {
        _circleAreaDamage.gameObject.SetActive(false);
        _rectangleAreaDamage.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        _megaPunchController = GetComponent<MegaPunchController>();
    }

    private void ChargeSuperPunch()
    {
        _chargeEffect.Play();
        _animator.StartPlayback();
        _scaleEffects = new Vector3(0, _maxSizeScaleChargingEffects, 0);
        StartCoroutine(_megaPunchController.ChargingMegaPunch(_rectangleAreaDamage, _scaleEffects));
    }

    private void SuperPunchDealDamage()
    {
        _chargeEffect.Stop();
        _rectangleShockWaveEffect.Play();
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit[] allHits = Physics.SphereCastAll(ray, 2.0f, _megaPunchController.DistanceMegaPunch * 2, LayerMask.GetMask("Player"));
        if (allHits.Length != 0)
        {
            foreach (var hit in allHits)
            {
                _megaPunchController.ShockWaveImpact(hit.collider);
            }
        }
        _megaPunchController.ReloadPunchEffect(_rectangleAreaDamage);
    }

    private void ChargeDoublePunch()
    {
        _chargeEffect.Play();
        _animator.StartPlayback();
        _scaleEffects = new Vector3(_maxSizeScaleChargingEffects, _maxSizeScaleChargingEffects, _maxSizeScaleChargingEffects);
        StartCoroutine(_megaPunchController.ChargingMegaPunch(_circleAreaDamage, _scaleEffects));
    }

    private void DoublePunchDealDamage()
    {
        _chargeEffect.Stop();
        _circleShockWaveEffect.Play();
        _hitColliders = Physics.OverlapSphere(transform.position, _megaPunchController.DistanceMegaPunch, LayerMask.GetMask("Player"));
        foreach (var collider in _hitColliders)
        {
            _megaPunchController.ShockWaveImpact(collider);
        }
        _megaPunchController.ReloadPunchEffect(_circleAreaDamage);
    }

    private void ChargeMagnettoPunch()
    {
        _magnettoEffect.Play();
        StartCoroutine(_megaPunchController.MagnettoModeOn());
    }

    private void MagnettoPunchDealDamage()
    {
        _magnettoEffect.Stop();
        _hitColliders = Physics.OverlapSphere(transform.position, _megaPunchController.DistanceMegaPunch, LayerMask.GetMask("Player"));
        foreach (var collider in _hitColliders)
        {
            _megaPunchController.ShockWaveImpact(collider);
        }
    }

    IEnumerator Relaxation()
    {
        _magnettoEffect.Stop();
        yield return new WaitForSeconds(_timeToRelax);
        _animator.CrossFade("Idle", 0.1f);
        _megaPunchController.StartCoroutine(_megaPunchController.PlayMegaPunch());
        StopAllCoroutines();
    }
}
