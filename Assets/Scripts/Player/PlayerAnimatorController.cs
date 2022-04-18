using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        MoveAnimations();
    }

    private void MoveAnimations()
    {
        _animator.SetFloat("Speed", _playerMovement.AnimationMovementValue.magnitude);
        _animator.SetFloat("x", _playerMovement.AnimationMovementValue.x);
        _animator.SetFloat("y", _playerMovement.AnimationMovementValue.y);
    }

    
}
