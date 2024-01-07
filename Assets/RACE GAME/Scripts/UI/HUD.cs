using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lapsTMP;
    [SerializeField] private TextMeshProUGUI _positionTMP;

    private void Awake()
    {
        _lapsTMP.SetText("0/3");
    }

    private void OnEnable()
    {
        ProgresEvents.OnLapCompleted += UpdateLapCounter;
        ProgresEvents.OnCheckpointReached += UpdatePositionCounter;
    }

    private void OnDisable()
    {
        ProgresEvents.OnLapCompleted -= UpdateLapCounter;
        ProgresEvents.OnCheckpointReached -= UpdatePositionCounter;
    }

    private void UpdateLapCounter(int lap)
    {
        _lapsTMP.SetText($"{lap}/3");
    }

    private void UpdatePositionCounter(int position, int totalRivals)
    {
        _positionTMP.SetText($"{position}/{totalRivals}");
    }
}
