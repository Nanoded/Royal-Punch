using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TrainingButton : MonoBehaviour
{
    [SerializeField] private int _trainingCost;
    [SerializeField] private CoinManager _coinManager;
    private Button _buttonComponent;

    private void Start()
    {
        _buttonComponent = GetComponent<Button>();
        EventsController.TrainingEvent.AddListener(Train);
    }

    private void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        if(PlayerPrefs.GetInt("Coins") < _trainingCost)
        {
            _buttonComponent.interactable = false;
        }
        else
        {
            _buttonComponent.interactable = true;
        }
    }

    private void Train()
    {
        _coinManager.SpendCoins(_trainingCost);
    }
}
