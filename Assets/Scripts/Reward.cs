using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewNamespace
{
    public class Reward : MonoBehaviour
    {

        private void Start()
        {
            transform.DORotate(Vector3.forward * 10, .5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
