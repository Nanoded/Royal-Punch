using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpectatorAnimationsController : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        int numberAnimation = Random.Range(1, 14);
        _animator.CrossFade($"{numberAnimation}", 0.1f);
    }

    void Update()
    {
        
    }
}
