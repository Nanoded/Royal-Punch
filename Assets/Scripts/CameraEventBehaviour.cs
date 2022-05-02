using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CameraEventBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainMenuCamera;
    [SerializeField] private CinemachineVirtualCamera _punchCamera;
    [SerializeField] private float _timePunchEffect;
    private PlayableDirector _playableDirector;

    private void Start()
    {
        _punchCamera.Priority = 0;
        _playableDirector = GetComponent<PlayableDirector>();
        EventsController.StartEvent.AddListener(StartGame);
        EventsController.RestartLoseEvent.AddListener(RestartGame);
        EventsController.RestartWinEvent.AddListener(RestartGame);
        Health.PlayerDeathEvent.AddListener(DeathEffectOn);
        Health.EnemyDeathEvent.AddListener(DeathEffectOn);
    }

    private void StartGame()
    {
        _mainMenuCamera.Priority = 0;
        _playableDirector.Play();
    }

    private void RestartGame()
    {
        _punchCamera.Priority = 0;
        _mainMenuCamera.Priority = 10;
    }

    private void DeathEffectOn()
    {
        _punchCamera.Priority = 10;
    }
}
