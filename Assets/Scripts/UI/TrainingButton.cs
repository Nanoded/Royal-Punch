using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TrainingButton : MonoBehaviour
{
    [SerializeField] private int _trainingCost;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private Button _buttonComponent;

    private void Start()
    {
        _buttonComponent.onClick.AddListener(Train);
    }

    private void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        if(_coinManager.CoinCount < _trainingCost)
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
