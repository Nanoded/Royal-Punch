using UnityEngine;

public class UIEventBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;

    void Start()
    {
        EventsController.StartEvent.AddListener(StartGame);
        EventsController.RestartLoseEvent.AddListener(RestartGame);
        EventsController.RestartWinEvent.AddListener(RestartGame);
        Health.EnemyDeathEvent.AddListener(ActivateWinPanel);
        Health.PlayerDeathEvent.AddListener(ActivateLosePanel);

        _startMenu.SetActive(true);
        _winMenu.SetActive(false);
        _loseMenu.SetActive(false);
    }

    private void StartGame()
    {
        _startMenu.SetActive(false);
    }
    private void RestartGame()
    {
        _loseMenu.SetActive(false);
        _winMenu.SetActive(false);
        _startMenu.SetActive(true);
    }
    private void ActivateWinPanel()
    {
        _winMenu.SetActive(true);
    }
    private void ActivateLosePanel()
    {
        _loseMenu.SetActive(true);
    }
}
