using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    public static UnityEvent RestartWinEvent = new UnityEvent();
    public static UnityEvent RestartLoseEvent = new UnityEvent();
    public static UnityEvent StartEvent = new UnityEvent();

    public void RestartWinGame()
    {
        RestartWinEvent.Invoke();
    }
    
    public void RestartLoseGame()
    {
        RestartLoseEvent.Invoke();
    }

    public void StartGame()
    {
        StartEvent.Invoke();
    }
}
