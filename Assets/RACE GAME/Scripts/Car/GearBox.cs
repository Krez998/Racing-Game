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

[RequireComponent(typeof(CarEngine), typeof(Rigidbody))]
public class GearBox : MonoBehaviour, IGearBox
{
    public int CurrentGear => _currentGear;
    public GearBoxMode GearBoxMode => _gearBoxMode;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;
    public float Speed => _speed;

    [Header("Gear Hud")]
    [SerializeField] private TextMeshProUGUI _gearText;


    [Header("Wheel Speed Params")]
    public float _speedMPS; // скорость колеса в м/с
    public float _wheelAngleSpeed; // углова€ скорость колеса м/с, –ад./с 
    public float _wheelMinAngularVelocity;
    public float _wheelMaxAngularVelocity; // скорость вращени€ колеса √рад./сек.


    [Header("Car Characteristics")]
    [SerializeField] private CarCharacteristics _carCharacteristics;
    [SerializeField, Tooltip("“екуща€ передача")] int _currentGear;

    public GearBoxMode _gearBoxMode;
    private Rigidbody _rigidbody;
    private CarEngine _engine;
    private WaitForSeconds _gearShiftDelay;
    public bool _isShifting;

    public float[] _speedValues;

    [Header("Gear Speed Limits")]
    [SerializeField] private float _currentGearMinSpeed;
    [SerializeField] private float _currentGearMaxSpeed;

    [SerializeField, Tooltip("“екуща€ скорость автомобил€")] private float _speed;
    public float Torque; // very demo


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _engine = GetComponent<CarEngine>();
        _gearShiftDelay = new WaitForSeconds(0.2f);
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
            if (Mathf.Round(transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f) < _speedValues[i])
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

                SetWheelAngularVelocity(_currentGearMaxSpeed);

                break;
            }
        }
    }

    public void GetEngineData(float motorTorque)
    {
        Torque = motorTorque;
        _speed = Mathf.Round(transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f);

        if (!_isShifting && _currentGear == 0 && _speed == 0 && motorTorque > 0)
        {
            // Debug.Log("Moving Away");
            StartCoroutine(SwitchGearbox(1));
        }

        if (!_isShifting && _speed > 0 && _speed >= _currentGearMaxSpeed - 3f)
        {
            //Debug.Log("Gear UP");
            StartCoroutine(SwitchGearbox(1));
        }

        if (!_isShifting && _speed > 0 && _speed < _currentGearMinSpeed - 3f)
        {
            //Debug.Log("Gear DOWN");
            StartCoroutine(SwitchGearbox(-1));
        }

        if (!_isShifting && motorTorque < 0 && _currentGear != -1)
        {
            //Debug.Log("R");
            StartCoroutine(SwitchGearbox(-1));
        }

        if (!_isShifting && motorTorque == 0 && _currentGear != 0 && _speed == 0)
        {
            //Debug.Log("Stop Car");
            StartCoroutine(SwitchGearbox(_currentGear = 0));
        }
    }

    private IEnumerator SwitchGearbox(int gear)
    {
        _isShifting = true;
        _gearBoxMode = GearBoxMode.Neutral;
        //Debug.Log("N");
        int tempGear = _currentGear;
        _currentGear = 0;
        UpdateGearText();
        yield return _gearShiftDelay;

        _currentGear = tempGear;
        _currentGear += gear;
        //Debug.Log("GEAR: " + _currentGear);

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
        UpdateGearText();

        _isShifting = false;

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
            if (_currentGear == -1)
                _gearText.text = "R";
            else if (_currentGear == 0)
                _gearText.text = "N";
            else
                _gearText.text = Numbers.GeneratedNumsStr[_currentGear];
        }
    }

    public float GetSpeed()
    {
        return _speed;
    }

    /// <summary>
    /// отображает центр массы автомобил€
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_rigidbody)
            Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
    }
}
