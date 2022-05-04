using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    private SlowMoEffect _slowMoEffect;
    private EventsController _eventsController;

    void Start()
    {
        _pausePanel.SetActive(false);
        _slowMoEffect = FindObjectOfType<SlowMoEffect>();
        _eventsController = FindObjectOfType<EventsController>();
    }

    public void PauseOn()
    {
        if(_pausePanel != null)
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void PauseOff()
    {
        if(_slowMoEffect != null)
        {
            _pausePanel.SetActive(false);
            Time.timeScale = _slowMoEffect.GameSpeed;
        }

    }

    public void ReturnToMainMenu()
    {
        _eventsController.RestartLoseGameButton();
        _pausePanel.SetActive(false);
    }
}
