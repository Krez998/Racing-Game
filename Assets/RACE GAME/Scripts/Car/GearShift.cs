using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CarEngine), typeof(Rigidbody))]
public class GearShift : MonoBehaviour // IGearShift
{
    public int CurrentGear => _currentGear;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;
    public float SpeedZ => _speedZ;
    public bool IsShifting => _isShifting;

    [Header("Gear Hud")]
    [SerializeField] private TextMeshProUGUI _gearText;

    [Header("Speed Params (Very Demo)")]
    public float _speedMPS;
    public float _wheelAngleSpeed; // угловая скорость колеса м/с, Рад./с 
    public float _wheelRotationSpeed; // скорость вращения колеса Град./сек.

    [SerializeField] private CarCharacteristics _carCharacteristics;
    [SerializeField] int _currentGear;
    private Rigidbody _rb;
    private CarEngine _engine;
    private WaitForSeconds _gearShiftDelay;
    public bool _isShifting;

    public float[] _speedValues;

    [Header("Gear Speed Limits")]
    [SerializeField] private float _currentGearMinSpeed;
    [SerializeField] private float _currentGearMaxSpeed;

    public float _speedZ;
    public float Torque;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _engine = GetComponent<CarEngine>();
        _gearShiftDelay = new WaitForSeconds(0.5f);
        InitSpeedValues();
    }

    private void Start()
    {
        StartSetGear();
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

    private void StartSetGear()
    {
        for (int i = 0; i < _speedValues.Length; i++)
        {
            if (Mathf.Round(transform.InverseTransformDirection(_rb.velocity).z * 3.6f) < _speedValues[i])
            {
                _currentGear = i;
                UpdateGearText();

                if (_currentGear != 0)
                {
                    _currentGearMinSpeed = _speedValues[i - 1];
                    _currentGearMaxSpeed = _speedValues[i];
                }
                else
                {
                    _currentGearMinSpeed = 0;
                    _currentGearMaxSpeed = 0;
                }

                CalculateWheelRotationSpeed();
                _engine.SetWheelAngularVelocity(_wheelRotationSpeed);

                break;
            }
        }
    }

    public void GetEngineData(float motorTorque)
    {
        Torque = motorTorque;
        _speedZ = Mathf.Round(transform.InverseTransformDirection(_rb.velocity).z * 3.6f);

        if (!_isShifting && _speedZ == 0 && motorTorque > 0 && _currentGear == 0)
        {
            Debug.Log("Moving Away");
            StartCoroutine(SetGear(++_currentGear));
        }

        if (!_isShifting && _speedZ > 0 && _speedZ >= _currentGearMaxSpeed - 3f)
        {
            Debug.Log("Gear UP");
            StartCoroutine(SetGear(++_currentGear));
        }

        if (!_isShifting && _speedZ > 0 && _speedZ < _currentGearMinSpeed - 3f)
        {
            Debug.Log("Gear DOWN");
            StartCoroutine(SetGear(--_currentGear));
        }

        if (!_isShifting && motorTorque < 0 && _currentGear != -1)
        {
            Debug.Log("R");
            StartCoroutine(SetGear(_currentGear = -1));
        }

        if (!_isShifting && motorTorque == 0 && _currentGear != 0 && _speedZ == 0)
        {
            Debug.Log("N");
            StartCoroutine(SetGear(_currentGear = 0));
        }
    }

    private IEnumerator SetGear(int gear)
    {
        _isShifting = true;
        _engine.IsAllowMove = false;
        UpdateGearText();
        yield return _gearShiftDelay;
        switch (gear)
        {
            case 1:
                _engine.IsAllowMove = true;
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = _speedValues[gear - 1];
                break;
            case 0:
                _engine.IsAllowMove = false;
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = 0;
                break;
            case -1:
                _engine.IsAllowMove = true;
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = -_speedValues[0];
                break;
            default:
                _engine.IsAllowMove = true;
                _currentGearMinSpeed = _speedValues[gear - 2];
                _currentGearMaxSpeed = _speedValues[gear - 1];
                break;
        }
        _isShifting = false;

        CalculateWheelRotationSpeed();
        _engine.SetWheelAngularVelocity(_wheelRotationSpeed);

        UpdateGearText();
    }

    //public void SetGear(int gear)
    //{
    //    switch (gear)
    //    {
    //        case 1:
    //            _currentGearMinSpeed = 0;
    //            _currentGearMaxSpeed = _speedValues[gear - 1];
    //            break;
    //        case 0:
    //            _currentGearMinSpeed = 0;
    //            _currentGearMaxSpeed = 0;
    //            break;
    //        case -1:
    //            _currentGearMinSpeed = 0;
    //            _currentGearMaxSpeed = -_speedValues[0];
    //            break;
    //        default:
    //            _currentGearMinSpeed = _speedValues[gear - 2];
    //            _currentGearMaxSpeed = _speedValues[gear - 1];
    //            break;
    //    }

    //    CalculateWheelRotationSpeed();
    //    _engine.SetWheelAngularVelocity(_wheelRotationSpeed);

    //    UpdateGearText();
    //}

    private void UpdateGearText()
    {
        if (_gearText)
        {
            if (_currentGear == -1)
                _gearText.text = "R";
            else if(_currentGear == 0)
                _gearText.text = "N";
            else if(_isShifting)
                _gearText.text = "N";
            else
                _gearText.text = Numbers.GeneratedNumsStr[_currentGear];
        }
    }

    private void CalculateWheelRotationSpeed()
    {
        _speedMPS = Mathf.Abs(_currentGearMaxSpeed) * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelRotationSpeed = _wheelAngleSpeed * Mathf.Rad2Deg;
    }
}
