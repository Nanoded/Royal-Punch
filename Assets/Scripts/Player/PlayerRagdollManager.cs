using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerRagdollManager : MonoBehaviour
{
    [SerializeField] GameObject _armature;
    private Rigidbody _playerRigidbody;
    private List<Rigidbody> _allBones = new List<Rigidbody>();
    private List<Vector3> _startPositionsBones = new List<Vector3>();
    private List<Quaternion> _startQuaternionsBones = new List<Quaternion>();
    private Animator _animator;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerRigidbody = GetComponent<Rigidbody>();
        GetAllRigidbodies(_armature);
        _armature.SetActive(false);
        Health.PlayerDeathEvent.AddListener(EnableRagdollAfterDeath);
        EventsController.RestartLoseEvent.AddListener(DisableRagdollAfterReset);
        EventsController.RestartWinEvent.AddListener(DisableRagdollAfterReset);
    }

    private void GetAllRigidbodies(GameObject parent)
    {
        foreach(Transform children in parent.transform)
        {
            if(children == null)
            {
                return;
            }

            if(children.TryGetComponent(out Rigidbody rigidbody))
            {
                _allBones.Add(rigidbody);
                _startQuaternionsBones.Add(rigidbody.transform.rotation);
                _startPositionsBones.Add(rigidbody.transform.localPosition);
            }

            GetAllRigidbodies(children.gameObject);
        }
    }

    private void EnableRagdollAfterDeath()
    {
        _armature.SetActive(true);
    }
    private void DisableRagdollAfterReset()
    {
        _armature.SetActive(false);
    }

    public void EnableRagdoll(float shockwaveForce, Vector3 directionShokwave)
    {
        _animator.enabled = false;
        _playerMovement.enabled = false;
        _armature.SetActive(true);
        _playerRigidbody.velocity = Vector3.zero;
        for (int i = 0; i < _allBones.Count; i++)
        {
            _allBones[i].AddForce(directionShokwave.normalized * shockwaveForce, ForceMode.Impulse);
        }
        StartCoroutine(DisableRagdoll());
    }

    IEnumerator DisableRagdoll()
    {
        yield return new WaitForSeconds(2);
        for(int i = 0; i < _allBones.Count; i++)
        {
            _allBones[i].transform.DOLocalMove(_startPositionsBones[i], 1);
            _allBones[i].transform.DORotateQuaternion(_startQuaternionsBones[i], 1);
        }
        yield return new WaitForSeconds(1);
        _armature.SetActive(false);
        _playerMovement.enabled = true;
        _animator.enabled = true;
        StopCoroutine(DisableRagdoll());
    }
}
