using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public enum CountdownFormatting { DaysHoursMinutesSeconds, HoursMinutesSeconds, MinutesSeconds, Seconds };
    public CountdownFormatting countdownFormatting = CountdownFormatting.MinutesSeconds; //Controls the way the timer string will be formatted
    public bool showMilliseconds = true; //Whether to show milliseconds in countdown formatting
    public double countdownTime = 6; //Countdown time in seconds
    [SerializeField] private GameObject _windowStart;
    [SerializeField] private Timer _timers;

    Text countdownText;
    double countdownInternal;
    bool countdownOver = false;



    // Start is called before the first frame update
    void Start()
    {
        countdownText = GetComponent<Text>();
        countdownInternal = countdownTime; //Initialize countdown
    }

    void FixedUpdate()
    {
        if (countdownInternal > 0)
        {
            countdownInternal -= Time.deltaTime;
            if (countdownInternal < 4 && countdownInternal > 0)
            {
                countdownText.text = FormatTime(countdownInternal);
            }
            else
            {
                countdownText.text = "";
            }
        }
        else
        {
            if (!countdownOver)
            {
                countdownOver = true;
                Debug.Log("Countdown has finished running...");
                _windowStart.SetActive(false);
                _timers.StartTimer();

            }
        }
    }

    //string FormatTime(double time, CountdownFormatting formatting, bool includeMilliseconds)
    string FormatTime(double time)
    {
        string timeText = "";

        int intTime = (int)time;

        int secondsTotal = intTime;

        timeText = string.Format("{0}", secondsTotal);

        return timeText;
    }
}