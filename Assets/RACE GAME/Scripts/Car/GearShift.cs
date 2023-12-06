using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CarEngine), typeof(Rigidbody))]
public class GearShift : MonoBehaviour // IGearShift
{
    public int CurrentGear => _currentGear;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;
    public float SpeedZ => _speedZ;

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



        if (_speedZ > 0 && _speedZ >= _currentGearMaxSpeed - 5f)
        {
            Debug.Log("Gear UP");
            SetGear(++_currentGear);
        }

        if (_speedZ > 0 && _speedZ < _currentGearMinSpeed - 5f)
        {
            Debug.Log("Gear DOWN");
            SetGear(--_currentGear);
        }

        if (motorTorque < 0 && _currentGear != -1)
        {
            Debug.Log("R");
            SetGear(_currentGear = -1);
        }

        if (motorTorque == 0 && _currentGear != 0 && _speedZ == 0)
        {
            Debug.Log("N");
            SetGear(_currentGear = 0);
        }
    }

    public void SetGear(int gear)
    {
        switch (gear)
        {
            case 1:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = _speedValues[gear - 1];
                break;
            case 0:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = 0;
                break;
            case -1:
                _currentGearMinSpeed = 0;
                _currentGearMaxSpeed = -_speedValues[0];
                break;
            default:
                _currentGearMinSpeed = _speedValues[gear - 2];
                _currentGearMaxSpeed = _speedValues[gear - 1];
                break;
        }

        CalculateWheelRotationSpeed();
        _engine.SetWheelAngularVelocity(_wheelRotationSpeed);

        UpdateGearText();
    }

    private void UpdateGearText()
    {
        if (_gearText)
        {
            if (_currentGear == -1)
                _gearText.text = "R";
            else if(_currentGear == 0)
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
