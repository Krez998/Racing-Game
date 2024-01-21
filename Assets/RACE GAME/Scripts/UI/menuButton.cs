using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButton : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TextMeshProUGUI _ratingText;

    private void Awake()
    {
        gameData.Load();
        _ratingText.SetText(gameData.Data.Rating.ToString());
    }

    public void LoadScene(int index)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(index);
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}