using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinCount;
    [SerializeField] private int _coinsForVictory;
    [SerializeField] private int _coinsForDefeat;

    private int _currentCoinCount;

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
        if(PlayerPrefs.HasKey("Coins"))
        {
            _currentCoinCount = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            _currentCoinCount = 0;
            PlayerPrefs.SetInt("Coins", _currentCoinCount);
        }
        PlayerPrefs.Save();
        _coinCount.text = _currentCoinCount.ToString();
    }

    private void AddWinCoin()
    {
        PlayerPrefs.SetInt("Coins", _currentCoinCount += _coinsForVictory);
        InitCoins();
    }
    
    private void AddLoseCoin()
    {
        PlayerPrefs.SetInt("Coins", _currentCoinCount += _coinsForDefeat);
        InitCoins();
    }

    public void SpendCoins(int price)
    {
        PlayerPrefs.SetInt("Coins", _currentCoinCount -= price);
        InitCoins();
    }
}
