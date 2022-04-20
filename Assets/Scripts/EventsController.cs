using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    public static UnityEvent RestartWinEvent = new UnityEvent();
    public static UnityEvent RestartLoseEvent = new UnityEvent();
    public static UnityEvent StartEvent = new UnityEvent();
    public static UnityEvent TrainingEvent = new UnityEvent();
    public static UnityEvent RemoveProgressEvent = new UnityEvent();

    public void RestartWinGameButton()
    {
        RestartWinEvent.Invoke();
    }

    public void RestartLoseGameButton()
    {
        RestartLoseEvent.Invoke();
    }

    public void StartGameButton()
    {
        StartEvent.Invoke();
    }

    public void TrainingButton()
    {
        TrainingEvent.Invoke();
    }

    public void RemoveProgressButton()
    {
        PlayerPrefs.DeleteAll();
        RemoveProgressEvent.Invoke();
    }
}
