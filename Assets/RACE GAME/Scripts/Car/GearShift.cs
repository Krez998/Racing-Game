using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CarEngine), typeof(Rigidbody))]
public class GearShift : MonoBehaviour // IGearShift
{
    [Header("Speed Km/h")]
    [SerializeField, Range(0, 300)] private float _speedKPH;

    [Header("Gear Hud")]
    [SerializeField] private TextMeshProUGUI _gearText;

    // временно
    [Header("Speed Params (Very Demo)")]
    public float _speedMPS;
    public float _wheelAngleSpeed; // углова€ скорость колеса м/с, –ад./с 
    public float _wheelRotationSpeed; // скорость вращени€ колеса √рад./сек.

    [SerializeField] private CarCharacteristics _carCharacteristics;
    [SerializeField] int _currentGear;
    private Rigidbody _rb;
    private CarEngine _engine;

    public float[] _speedValues;

    [Header("Gear Speed Limits")]
    public float _currGearMinSpeed;
    public float _currGearMaxSpeed;

    public float _speedZ;
    public float Torque;


    private Vector3 _lastPosition;
    private Vector3 _differencePosition;

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
        _speedValues = new float[_carCharacteristics.NumberOfGears + 1];
        float speedDelta = _carCharacteristics.Speed / _carCharacteristics.NumberOfGears;

        float speedValueGrowth = 0;
        for (int i = 1; i < _speedValues.Length; i++)
        {
            speedValueGrowth += speedDelta;
            _speedValues[i] = speedValueGrowth;
        }

        _speedValues[0] = speedDelta;
    }

    private void StartSetGear()
    {
        for (int i = 1; i < _speedValues.Length; i++)
        {
            if (_rb.velocity.magnitude < _speedValues[i])
            {
                _currentGear = i;
                UpdateGearText();

                if (_currentGear != 1)
                    _currGearMinSpeed = _speedValues[i - 1];
                else
                    _currGearMinSpeed = 0;

                _currGearMaxSpeed = _speedValues[i];

                CalculateWheelRotationSpeed();
                _engine.SetWheelAngularVelocity(_wheelRotationSpeed);

                break;
            }
        }
    }

    public void GetEngineData(float motorTorque)
    {
        //Vector3 differencePosition = transform.position - _lastPosition;
        //differencePosition = transform.InverseTransformDirection(differencePosition);

        Torque = motorTorque;
        _speedZ = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;


        if (_speedZ > 0 && _speedZ >= _currGearMaxSpeed - 5f)
        {
            if (_currentGear == -1) _currentGear = 0;
            Debug.Log("Gear UP");
            SetGear(++_currentGear);
        }

        if (_speedZ > 0 && _speedZ < _currGearMinSpeed - 5f)
        {
            Debug.Log("Gear DOWN");
            SetGear(--_currentGear);
        }

        if (motorTorque < 0 && _speedZ < 0)
        {
            Debug.Log("R");
            SetGear(_currentGear = 0);
        }

        if (motorTorque == 0 && Math.Round(_speedZ, 2) == 0)
        {
            Debug.Log("N");
            SetGear(_currentGear = -1);
        }


        //_lastPosition = transform.position;
    }

    public void SetGear(int gear)
    {
        Debug.Log("SetGear: " + gear);

        if (gear != -1)
        {
            if (gear == 1 || gear == 0)
                _currGearMinSpeed = 0;
            else
                _currGearMinSpeed = _speedValues[gear - 1];

            _currGearMaxSpeed = _speedValues[gear];

            CalculateWheelRotationSpeed();
            _engine.SetWheelAngularVelocity(_wheelRotationSpeed);
        }
        else
            _currGearMaxSpeed = 0;

        UpdateGearText();
    }

    private void UpdateGearText()
    {
        if (_gearText)
        {
            if (_currentGear == 0)
                _gearText.text = "R";
            else if(_currentGear == -1)
                _gearText.text = "N";
            else
                //_gearText.text = Numbers.GeneratedNumsStr[_currentGear];
                _gearText.text = _currentGear.ToString();
        }
    }

    private void CalculateWheelRotationSpeed()
    {
        _speedMPS = _currGearMaxSpeed * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelRotationSpeed = _wheelAngleSpeed * Mathf.Rad2Deg;
    }

    
    //private void FixedUpdate()
    //{
    //    if (_rb.velocity.magnitude * 3.6f >= _currGearMaxSpeed - 5f)
    //    {
    //        _currentGear++;
    //        SetGear();
    //    }

    //    if (_rb.velocity.magnitude * 3.6f < _currGearMinSpeed - 5f)
    //    {
    //        _currentGear--;
    //        SetGear();
    //    }

    //    if (_engine.MotorTorque < 0)
    //    {
    //        _currentGear = 0;
    //        SetReverseGear();
    //        UpdateGearText();
    //    }
    //    else
    //    {
    //        UpdateGearText();
    //    }
    //}

    //public void SetGear()
    //{
    //    Debug.Log("SetGear");
    //    //UpdateGearText();

    //    if (_currentGear != 1)
    //        _currGearMinSpeed = _speedValues[_currentGear - 1];
    //    else
    //        _currGearMinSpeed = 0;

    //    _currGearMaxSpeed = _speedValues[_currentGear];
    //    CalculateWheelRotationSpeed();
    //    _engine.SetWheelAngularVelocity(_wheelRotationSpeed);
    //}

    //public void SetReverseGear()
    //{
    //    _currentGear = 0;
    //    _currGearMaxSpeed = _speedValues[_currentGear];
    //    CalculateWheelRotationSpeed();
    //    _engine.SetWheelAngularVelocity(_wheelRotationSpeed);
    //}

    //private void CalculateWheelRotationSpeed()
    //{
    //    _speedMPS = _currGearMaxSpeed * 1000 / 3600;
    //    _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
    //    _wheelRotationSpeed = _wheelAngleSpeed * Mathf.Rad2Deg;
    //}

    //private void UpdateGearText()
    //{
    //    if (_gearText)
    //    {
    //        if (_currentGear == 0)
    //            _gearText.text = "R";
    //        else
    //            _gearText.text = Numbers.GeneratedNumsStr[_currentGear];
    //    }
    //}

}
