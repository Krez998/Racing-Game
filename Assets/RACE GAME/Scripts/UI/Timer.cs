using System;
using TMPro;
using UnityEngine;
using System.Diagnostics;



public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private int _milliseconds;
    [SerializeField] private int _seconds;
    [SerializeField] private int _minutes;
    [SerializeField] private int _hours;
    public static bool GameIsPaused = true;

    private Stopwatch stopwatch = new Stopwatch();
    private char[] _chars;

    private void Awake()
    {
        _chars = new char[12] { '0', '0', ':', '0', '0', ':', '0', '0', '.', '0', '0', '0' };

    }

    public void StartTimer()
    {
        stopwatch.Start();
        GameIsPaused = true;
    }

    public void StopTimer()
    {
        stopwatch.Stop();
        GameIsPaused = false;
    }

    private void Update()
    {
        UpdateTimer();
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                StopTimer();
            } else {
                StartTimer();
            }
        }
    }

    private void UpdateTimer()
    {
        _chars[0] = (char)((stopwatch.Elapsed.Hours / 10) + 48);
        _chars[1] = (char)((stopwatch.Elapsed.Hours % 10) + 48);

        _chars[3] = (char)((stopwatch.Elapsed.Minutes / 10) + 48);
        _chars[4] = (char)((stopwatch.Elapsed.Minutes % 10) + 48);

        _chars[6] = (char)((stopwatch.Elapsed.Seconds / 10) + 48);
        _chars[7] = (char)((stopwatch.Elapsed.Seconds % 10) + 48);

        _chars[9] = (char)((stopwatch.Elapsed.Milliseconds / 100) + 48);
        _chars[10] = (char)(((stopwatch.Elapsed.Milliseconds % 100) / 10) + 48);
        _chars[11] = (char)((stopwatch.Elapsed.Milliseconds % 10) + 48);

        _milliseconds = stopwatch.Elapsed.Milliseconds;
        _seconds = stopwatch.Elapsed.Seconds;
        _minutes = stopwatch.Elapsed.Minutes;
        _hours = stopwatch.Elapsed.Hours;

        _textMeshPro.SetCharArray(_chars, 0, _chars.Length);
    }
 
    public TimeSpan GetData()
    {
        var _timeInSurvival = new TimeSpan(_hours, _minutes, _seconds);
        return _timeInSurvival;
    }
}