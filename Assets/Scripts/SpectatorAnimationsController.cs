using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpectatorAnimationsController : MonoBehaviour
{
    [SerializeField] private int _animationsCount;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        int numberAnimation = Random.Range(1, _animationsCount + 1);
        _animator.CrossFade($"{numberAnimation}", 0.1f);
    }
}
