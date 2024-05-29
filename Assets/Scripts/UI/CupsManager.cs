using TMPro;
using UnityEngine;
using YG;

namespace NewNamespace
{
    public class CupsManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _cupsCount;
        private int _currentCupsCount;

        private void Awake()
        {
            YandexGame.GetDataEvent += () => _currentCupsCount = YandexGame.savesData.Cups;
            _currentCupsCount = YandexGame.savesData.Cups;
            Health.EnemyDeathEvent.AddListener(() =>
            {
                _currentCupsCount++;
                _cupsCount.text = _currentCupsCount.ToString();
                YandexGame.savesData.Cups = _currentCupsCount;
                YandexGame.SaveProgress();
                YandexGame.NewLeaderboardScores("Top", _currentCupsCount);
            });
            _cupsCount.text = _currentCupsCount.ToString();
        }
    }
}
