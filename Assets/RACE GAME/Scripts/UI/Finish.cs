using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

using System;


public class Finish : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _position;
    [SerializeField] public GameObject _windowFinish;
    private int Position;

    public void checkLaps(int position)
    {
        _position.SetText("");
        Position = position;
        Update();
    }

    void Update()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        _windowFinish.SetActive(true);
        _position.SetText("         " + Position.ToString());
        
    }
}
