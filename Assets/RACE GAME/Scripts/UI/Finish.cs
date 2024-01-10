using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionTMP;
    [SerializeField] private TextMeshProUGUI _rewardTMP;

    [SerializeField] private GameObject _windowFinish;
    [SerializeField] private Timer _timer;

    private void Awake()
    {
        _positionTMP.SetText("");
        _rewardTMP.SetText("");
    }

    private void OnEnable()
    {
        GameEvents.OnGameFinished += OnGameFinished;
    }

    private void OnDisable()
    {
        GameEvents.OnGameFinished -= OnGameFinished;
    }

    public void OnGameFinished(int position, int reawrd)
    {
        _windowFinish.SetActive(true);
        _timer.StopTimer();
        AudioListener.pause = true;
        _positionTMP.SetText(position.ToString());
        _rewardTMP.SetText(reawrd.ToString());
        Time.timeScale = 0f;
    }
}
