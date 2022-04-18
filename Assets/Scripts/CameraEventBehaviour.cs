using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraEventBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainMenuCamera;
    private PlayableDirector _playableDirector;

    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        EventsController.StartEvent.AddListener(StartGame);
        EventsController.RestartLoseEvent.AddListener(RestartGame);
        EventsController.RestartWinEvent.AddListener(RestartGame);
    }

    private void StartGame()
    {
        _mainMenuCamera.Priority = 0;
        _playableDirector.Play();
    }

    private void RestartGame()
    {
        _mainMenuCamera.Priority = 10;
    }
}
