using UnityEngine;

namespace NewScripts.Player
{
    public class Animations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Movement _movement;

        private void Update()
        {
            _animator.SetFloat("Speed", _movement.Direction.magnitude);
            _animator.SetFloat("x", _movement.Direction.x);
            _animator.SetFloat("y", _movement.Direction.z);
        }
    }
}
