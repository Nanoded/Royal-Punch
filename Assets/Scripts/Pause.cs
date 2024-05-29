using UnityEngine;
using YG;

namespace AA
{
    public class Pause : MonoBehaviour
    {
        private bool _canWork;
        private object _locker = new();
        private object _locker2 = new();

        private void Awake ()
        {
            _canWork = true;
            YandexGame.OpenFullAdEvent += () => 
            {
                lock(_locker)
                {
                    _canWork = false;
                }

            };
            YandexGame.CloseFullAdEvent += () => 
            {
                lock(_locker2)
                {
                    _canWork = true;
                }
            };
        }

        private void OnApplicationFocus(bool focus)
        {
            if (_canWork)
                Enable(focus);
        }

        public void Enable(bool isActive)
        {
            if(isActive)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
            else
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
        }
    }
}

