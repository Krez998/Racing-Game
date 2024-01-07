using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lapsTMP;
    [SerializeField] private TextMeshProUGUI _positionTMP;
    [SerializeField] private TextMeshProUGUI _spedometerTMP;
    [SerializeField] private Image _speedometerFill;
    [SerializeField] private TextMeshProUGUI _gearTMP;

    private void Awake()
    {
        _lapsTMP.SetText("0/3");
    }

    private void OnEnable()
    {
        GameEvents.OnLapCompleted += UpdateLapCounter;
        GameEvents.OnCheckpointReached += UpdatePositionCounter;
        GameEvents.OnSpeedometerUpdating += UpdateSpeedometer;
        GameEvents.OnSpeedIndicatorUpdating += UpdateSpeedIndicator;
        GameEvents.OnGearShifted += UpdateGearCounter;
    }

    private void OnDisable()
    {
        GameEvents.OnLapCompleted -= UpdateLapCounter;
        GameEvents.OnCheckpointReached -= UpdatePositionCounter;
        GameEvents.OnSpeedometerUpdating -= UpdateSpeedometer;
        GameEvents.OnSpeedIndicatorUpdating -= UpdateSpeedIndicator;
        GameEvents.OnGearShifted -= UpdateGearCounter;
    }

    private void UpdateLapCounter(int lap)
    {
        _lapsTMP.SetText($"{lap}/3");
    }

    private void UpdatePositionCounter(int position, int totalRivals)
    {
        _positionTMP.SetText($"{position}/{totalRivals}");
    }

    private void UpdateSpeedometer(float speedABS)
    {
        _spedometerTMP.SetText(Numbers.CachedNums[(int)speedABS]);
    }

    private void UpdateSpeedIndicator(float speed, GearBoxMode gearBoxMode, float currentGearMinSpeed, float currentGearMaxSpeed)
    {
        if (gearBoxMode == GearBoxMode.Neutral && (int)speed == 0)
        {
            _speedometerFill.fillAmount = 0f;
        }
        else
        {
            var value = currentGearMinSpeed;
            var amount = Mathf.Abs(speed - value) / (Mathf.Abs(currentGearMaxSpeed) - value);
            _speedometerFill.fillAmount = amount;
        }
    }

    private void UpdateGearCounter(int gear)
    {
        if(gear == -1)
        {
            _gearTMP.SetText("R");
        }
        else if(gear == 0)
        {
            _gearTMP.SetText("N");
        }
        else
        {
            _gearTMP.SetText(Numbers.CachedNums[gear]);
        }
    }
}
