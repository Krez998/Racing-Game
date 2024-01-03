using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Diagnostics;

public class Timer : MonoBehaviour
{
    [SerializeField, Range(1,100)] private float TIME_SPEED;

    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private int _milliseconds;
    [SerializeField] private int _seconds;
    [SerializeField] private int _minutes;
    [SerializeField] private int _hours;

    private WaitForSeconds _timerDelay;
    private TimeSpan _timeInSurvival;
    private Coroutine _coroutine;
    private Stopwatch stopwatch = new Stopwatch();
    private char[] _chars;

    private void Awake()
    {
        _timerDelay = new WaitForSeconds(1f);
        _chars = new char[12] { '0', '0', ':', '0', '0', ':', '0', '0', '.', '0', '0', '0' };

        //_chars = new char[12] { ' ', ' ', ' ', ' ', '0', ':', '0', '0', '.', '0', '0', '0' };

        //StartTimer();
        stopwatch.Start();
    }

    private void Update()
    {
        //Time.timeScale = TIME_SPEED;
        UpdateTimer();
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

        _textMeshPro.SetCharArray(_chars, 0, _chars.Length);
    }

    private void BeautifulTimer()
    {
        _chars[0] = (char)((stopwatch.Elapsed.Hours / 10) + 48) == '0' ? ' ' : (char)((stopwatch.Elapsed.Hours / 10) + 48);
        _chars[1] = (char)((stopwatch.Elapsed.Hours % 10) + 48) == '0' ? ' ' : (char)((stopwatch.Elapsed.Hours % 10) + 48);
        _chars[2] = _chars[1] == '0' ? ' ' : ':';

        _chars[3] = (char)((stopwatch.Elapsed.Minutes / 10) + 48) == '0' ? ' ' : (char)((stopwatch.Elapsed.Minutes / 10) + 48);
        _chars[4] = (char)((stopwatch.Elapsed.Minutes % 10) + 48);

        _chars[6] = (char)((stopwatch.Elapsed.Seconds / 10) + 48);
        _chars[7] = (char)((stopwatch.Elapsed.Seconds % 10) + 48);

        _chars[9] = (char)((stopwatch.Elapsed.Milliseconds / 100) + 48);
        _chars[10] = (char)(((stopwatch.Elapsed.Milliseconds % 100) / 10) + 48);
        _chars[11] = (char)((stopwatch.Elapsed.Milliseconds % 10) + 48);

        _textMeshPro.SetCharArray(_chars, 0, _chars.Length);
    }

    //public void StartTimer()
    //{
    //    _coroutine = StartCoroutine(CoroutineTimer());
    //}

    //public void StopTimer()
    //{
    //    StopCoroutine(_coroutine);
    //}

    //private IEnumerator CoroutineTimer()
    //{
    //    while (true)
    //    {
    //        _seconds++;
    //        if (_seconds == 60)
    //        {
    //            _seconds = 0;
    //            _minutes++;
    //            if (_minutes == 60)
    //            {
    //                _minutes = 0;
    //                if (_hours == 99) yield break;
    //                _hours++;
    //                _chars[0] = (char)((_hours / 10) + 48);
    //                _chars[1] = (char)((_hours % 10) + 48);
    //                _chars[3] = (char)(0 + 48);
    //                _chars[4] = (char)(0 + 48);
    //            }
    //            _chars[3] = (char)((_minutes / 10) + 48);
    //            _chars[4] = (char)((_minutes % 10) + 48);
    //        }
    //        _chars[6] = (char)((_seconds / 10) + 48);
    //        _chars[7] = (char)((_seconds % 10) + 48);

    //        _textMeshPro.SetCharArray(_chars, 0, _chars.Length);
    //        yield return _timerDelay;
    //    }
    //}


    public TimeSpan GetData()
    {
        _timeInSurvival = new TimeSpan(_hours, _minutes, _seconds);
        return _timeInSurvival;
    }
}