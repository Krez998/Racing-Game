using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum GearBoxMode
{
    Neutral,
    Forward,
    Backward
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(Speedometer))]
public class GearBox : MonoBehaviour, IGearBox
{
    public int CurrentGear => _currentGear;
    public GearBoxMode GearBoxMode => _gearBoxMode;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;
    public float CurrentGearMinSpeed => _currentGearMinSpeed;

    [Header("Gear Hud")]
    [SerializeField] private TextMeshProUGUI _gearText;

    [Header("Wheel Speed Params")]
    [SerializeField] private float _speedMPS; // скорость колеса в м/с
   
    [SerializeField] private float _wheelAngleSpeed; // углова€ скорость колеса м/с, –ад./с
    [Header("MIN MAX")]
    [SerializeField] private float _wheelMinAngularVelocity;
    [SerializeField] private float _wheelMaxAngularVelocity; // скорость вращени€ колеса √рад./сек.

    [Header("Gear Settings")]
    [SerializeField] private int _currentGear;
    [SerializeField] private int _maxGear;

    [Header("Gear Speed Limits")]
    [SerializeField] private float _currentGearMinSpeed;
    [SerializeField] private float _currentGearMaxSpeed;

    [Header("Engine Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _motorTorque;

    private GearBoxMode _gearBoxMode;
    private CarEngine _engine;
    private Speedometer _speedometer;
    private WaitForSeconds _gearShiftDelay;
    [SerializeField] private bool _isShifting;

    [SerializeField] private float[] _speedValues;
    
    private void Awake()
    {
        _engine = GetComponent<CarEngine>();
        _speedometer = GetComponent<Speedometer>();
        _gearShiftDelay = new WaitForSeconds(0.2f);
        StartCoroutine(SetGear(0));
    }

    private void FixedUpdate()
    {
        _speed = _speedometer.GetSpeed();
        ChangeGears();
    }

    public void GetData(float speed, int numberOfGears)
    {
        _maxGear = numberOfGears;
        _speedValues = new float[_maxGear];
        float speedDelta = speed / _maxGear;

        float speedValueGrowth = 0;
        for (int i = 0; i < _speedValues.Length; i++)
        {
            speedValueGrowth += speedDelta;
            _speedValues[i] = speedValueGrowth;
        }
    }

    private void ChangeGears()
    {
        _motorTorque = _engine.MotorTorque;

        if (!_isShifting && (_currentGear == 0 || _currentGear == -1) && _speed == 0 && _motorTorque > 0)
        {
            //Debug.Log("Moving Away");
            StartCoroutine(SetGear(1));
        }

        if (!_isShifting && _speedometer.MovesInForwardDirection && _speed >= _currentGearMaxSpeed - 3f && _currentGear != _maxGear)
        {
            //Debug.Log("Gear UP");
            StartCoroutine(SetGear(++_currentGear));
        }

        if (!_isShifting && _speedometer.MovesInForwardDirection && _speed < _currentGearMinSpeed - 3f)
        {
            //Debug.Log("Gear DOWN");
            StartCoroutine(SetGear(--_currentGear));
        }

        if (!_isShifting && _motorTorque < 0 && _currentGear != -1)
        {
            //Debug.Log("R");
            StartCoroutine(SetGear(-1));
        }

        if (!_isShifting && _motorTorque == 0 && _currentGear != 0 && _speed == 0)
        {
            //Debug.Log("Stop Car & N");
            StartCoroutine(SetGear(0));
        }
    }

    private IEnumerator SetGear(int gear)
    {
        _isShifting = true;
        _gearBoxMode = GearBoxMode.Neutral;
        _currentGear = 0;
        UpdateGearText();
        yield return _gearShiftDelay;

        _currentGear = gear;

        switch (_currentGear)
        {
            case 1:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = _speedValues[_currentGear - 1];
                _gearBoxMode = GearBoxMode.Forward;
                break;
            case 0:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = 0;
                _gearBoxMode = GearBoxMode.Neutral;
                break;
            case -1:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = -_speedValues[0];
                _gearBoxMode = GearBoxMode.Backward;
                break;
            default:
                _currentGearMinSpeed = _speedValues[_currentGear - 2];
                _currentGearMaxSpeed = _speedValues[_currentGear - 1];
                _gearBoxMode = GearBoxMode.Forward;
                break;
        }

        _isShifting = false;
        UpdateGearText();
        SetWheelAngularVelocity(_currentGearMinSpeed, _currentGearMaxSpeed);
        _engine.ResetGasInput();
    }

    private void SetWheelAngularVelocity(float currentGearMinSpeed, float currentGearMaxSpeed)
    {
        //_wheelMinAngularVelocity = _wheelMaxAngularVelocity;

        _speedMPS = currentGearMinSpeed * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelMinAngularVelocity = _wheelAngleSpeed * Mathf.Rad2Deg;

        _speedMPS = currentGearMaxSpeed * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelMaxAngularVelocity = _wheelAngleSpeed * Mathf.Rad2Deg;

        //if (_wheelMinAngularVelocity > _wheelMaxAngularVelocity)
        //    _wheelMinAngularVelocity -= _wheelMaxAngularVelocity;

        _engine.SetWheelAngularVelocity(_wheelMinAngularVelocity, _wheelMaxAngularVelocity);
    }

    private void UpdateGearText()
    {
        if (_gearText)
        {
            if (_gearBoxMode == GearBoxMode.Backward)
                _gearText.text = "R";
            else if (_gearBoxMode == GearBoxMode.Neutral)
                _gearText.text = "N";
            else
                _gearText.text = Numbers.CachedNums[_currentGear];
        }
    }
}
