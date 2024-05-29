using UnityEngine;
using TMPro;
using YG;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinCount;
    [SerializeField] private int _coinsForVictory;
    [SerializeField] private int _coinsForDefeat;

    private int _currentCoinCount;

    public int CoinCount => _currentCoinCount;

    private void Awake()
    {
        YandexGame.GetDataEvent += InitCoins;
    }

    private void Start()
    {
        InitCoins();
        Health.EnemyDeathEvent.AddListener(AddWinCoin);
        Health.PlayerDeathEvent.AddListener(AddLoseCoin);
        EventsController.RestartLoseEvent.AddListener(InitCoins);
        EventsController.RestartWinEvent.AddListener(InitCoins);
        EventsController.RemoveProgressEvent.AddListener(InitCoins);
    }

    private void InitCoins()
    {
        _currentCoinCount = YandexGame.savesData.Coins;
        _coinCount.text = _currentCoinCount.ToString();
    }

    public void AddWinCoin()
    {
        _currentCoinCount += _coinsForVictory;
        YandexGame.savesData.Coins = _currentCoinCount;
        YandexGame.SaveProgress();
        InitCoins();
    }
    
    private void AddLoseCoin()
    {
        _currentCoinCount += _coinsForDefeat;
        YandexGame.savesData.Coins = _currentCoinCount;
        YandexGame.SaveProgress();
        InitCoins();
    }

    public void SpendCoins(int price)
    {
        _currentCoinCount -= price;
        YandexGame.savesData.Coins = _currentCoinCount;
        YandexGame.SaveProgress();
        InitCoins();
    }
}
