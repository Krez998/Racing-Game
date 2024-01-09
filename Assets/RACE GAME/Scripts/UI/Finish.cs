using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _position;
    [SerializeField] public TextMeshProUGUI _rewardTMP;

    [SerializeField] public GameObject _windowFinish;
    [SerializeField] private Timer _timer;
    [SerializeField] private Level _level;

    private void Awake()
    {
        _position.SetText("");
        _rewardTMP.SetText("");
    }

    public void OpenWindowFinish(int position)
    {
        _timer.StopTimer();
        Time.timeScale = 0f;
        AudioListener.pause = true;
        _windowFinish.SetActive(true);
        _position.SetText(position.ToString());
        _rewardTMP.SetText(_level.Reward.ToString());
    }
}
