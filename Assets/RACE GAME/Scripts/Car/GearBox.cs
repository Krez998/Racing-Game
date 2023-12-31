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

[RequireComponent(typeof(Rigidbody), typeof(CarEngine), typeof(ISpeedometer))]
public class GearBox : MonoBehaviour, IGearBox
{
    public int CurrentGear => _currentGear;
    public GearBoxMode GearBoxMode => _gearBoxMode;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;

    [Header("Gear Hud")]
    [SerializeField] private TextMeshProUGUI _gearText;

    [Header("Wheel Speed Params")]
    public float _speedMPS; // скорость колеса в м/с
    public float _wheelAngleSpeed; // угловая скорость колеса м/с, Рад./с 
    public float _wheelMinAngularVelocity;
    public float _wheelMaxAngularVelocity; // скорость вращения колеса Град./сек.

    [Header("Car Characteristics")]
    [SerializeField] private CarCharacteristics _carCharacteristics;
    [SerializeField] int _currentGear;

    public GearBoxMode _gearBoxMode;
    private Rigidbody _rigidbody;
    private CarEngine _engine;
    private ISpeedometer _speedometer;
    private WaitForSeconds _gearShiftDelay;
    [SerializeField] private bool _isShifting;

    [SerializeField] private float[] _speedValues;

    [Header("Gear Speed Limits")]
    [SerializeField] private float _currentGearMinSpeed;
    [SerializeField] private float _currentGearMaxSpeed;

    [Header("Engine Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _motorTorque;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _engine = GetComponent<CarEngine>();
        _speedometer = GetComponent<ISpeedometer>();
        _gearShiftDelay = new WaitForSeconds(0.2f);
        InitSpeedValues();
    }

    private void Start()
    {
        StartCoroutine(SetGear(0));
    }

    private void Update()
    {
        _speed = _speedometer.GetSpeed();
        ChangeGears();
    }

    private void InitSpeedValues()
    {
        _speedValues = new float[_carCharacteristics.NumberOfGears];
        float speedDelta = _carCharacteristics.Speed / _carCharacteristics.NumberOfGears;

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

        if (!_isShifting && _speed > 0 && _speed >= _currentGearMaxSpeed - 3f)
        {
            //Debug.Log("Gear UP");
            StartCoroutine(SetGear(++_currentGear));
        }

        if (!_isShifting && _speed > 0 && _speed < _currentGearMinSpeed - 3f)
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
            //Debug.Log("Stop Car");
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
        SetWheelAngularVelocity(_currentGearMaxSpeed);
        _engine.ResetGasInput();
    }

    private void SetWheelAngularVelocity(float speed)
    {
        _wheelMinAngularVelocity = _wheelMaxAngularVelocity;

        _speedMPS = Mathf.Abs(speed) * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelMaxAngularVelocity = _wheelAngleSpeed * Mathf.Rad2Deg;

        if (_wheelMinAngularVelocity > _wheelMaxAngularVelocity)
            _wheelMinAngularVelocity -= _wheelMaxAngularVelocity;

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

    /// <summary>
    /// отображает центр массы автомобиля
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_rigidbody)
            Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
    }
}
