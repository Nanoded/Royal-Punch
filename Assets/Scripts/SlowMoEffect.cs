using UnityEngine;

public class SlowMoEffect : MonoBehaviour
{
    [SerializeField] private float _slowMoTimeScale;
    [SerializeField] private float _gameSpeed;
    [SerializeField] private AudioSource _endSound;
    [SerializeField] private AudioSource _peopleSound;

    public float GameSpeed => _gameSpeed;

    void Start()
    {
        EventsController.RestartLoseEvent.AddListener(ResetSettings);
        EventsController.RestartWinEvent.AddListener(ResetSettings);
        Health.PlayerDeathEvent.AddListener(SlowMoEffectOn);
        Health.EnemyDeathEvent.AddListener(SlowMoEffectOn);
        ResetSettings();
    }

    private void ResetSettings()
    {
        Time.timeScale = _gameSpeed;
    }

    private void SlowMoEffectOn()
    {
        _endSound.Play();
        _peopleSound.Play();
        Time.timeScale = _slowMoTimeScale;
    }
}
