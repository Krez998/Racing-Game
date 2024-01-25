using System.Collections;
using UnityEngine;

public enum GearBoxMode
{
    Neutral,
    Forward,
    Backward
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
public class GearBox : MonoBehaviour
{
    public int CurrentGear => _currentGear;
    public GearBoxMode GearBoxMode => _gearBoxMode;
    public float Speed => _speed;
    public bool IsMovingInForwardDirection => _isMovingInForwardDirection;
    public float CurrentGearMaxSpeed => _currentGearMaxSpeed;
    public float CurrentGearMinSpeed => _currentGearMinSpeed;

    [SerializeField] private bool _isShifting;
    [SerializeField] private float[] _speedValues;

    [Header("Wheel Speed Params")]
    [SerializeField] private float _speedMPS; // скорость колеса в м/с
    [SerializeField] private float _wheelAngleSpeed; // углова€ скорость колеса м/с, –ад./с

    [Header("MIN MAX")]
    [SerializeField] private float _wheelMinRotationSpeed;
    [SerializeField] private float _wheelMaxRotationSpeed; // скорость вращени€ колеса √рад./сек.

    [Header("Gear Settings")]
    [SerializeField] private int _currentGear;
    [SerializeField] private int _maxGear;

    [Header("Gear Speed Limits")]
    [SerializeField] private float _currentGearMinSpeed;
    [SerializeField] private float _currentGearMaxSpeed;

    [Header("Info")]
    [SerializeField] private float _speed;
    [SerializeField] private float _motorTorque;
    [SerializeField] private bool _isMovingInForwardDirection;

    private GearBoxMode _gearBoxMode;
    private Rigidbody _rigidbody;
    private CarEngine _engine;
    private WaitForSeconds _gearShiftDelay;
    private bool _isPlayerCar;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _engine = GetComponent<CarEngine>();
        _gearShiftDelay = new WaitForSeconds(0.2f);
        StartCoroutine(SetGear(0));
    }

    private void FixedUpdate()
    {
        _speed = Mathf.Round(transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f);
        _isMovingInForwardDirection = _speed > 0 ? true : false;
        ChangeGears();
    }

    public void GetData(bool isPlayerCar, float speed, int numberOfGears)
    {
        _isPlayerCar = isPlayerCar;
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

        if (!_isShifting && _isMovingInForwardDirection && _speed >= _currentGearMaxSpeed - 3f && _currentGear != _maxGear)
        {
            //Debug.Log("Gear UP");
            StartCoroutine(SetGear(++_currentGear));
        }

        if (!_isShifting && _isMovingInForwardDirection && _speed < _currentGearMinSpeed - 3f)
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
        if (_isPlayerCar)
            GameEvents.OnGearShifted?.Invoke(_currentGear);
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
        if (_isPlayerCar)
            GameEvents.OnGearShifted?.Invoke(_currentGear);
        SetWheelsRotationSpeed(_currentGearMinSpeed, _currentGearMaxSpeed);
        _engine.ResetGasInput();
    }

    private void SetWheelsRotationSpeed(float currentGearMinSpeed, float currentGearMaxSpeed)
    {
        //_wheelMinAngularVelocity = _wheelMaxAngularVelocity;

        _speedMPS = currentGearMinSpeed * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelMinRotationSpeed = _wheelAngleSpeed * Mathf.Rad2Deg;

        _speedMPS = currentGearMaxSpeed * 1000 / 3600;
        _wheelAngleSpeed = _speedMPS / _engine.DrivingWheels[0].WheelCollider.radius;
        _wheelMaxRotationSpeed = _wheelAngleSpeed * Mathf.Rad2Deg;

        //if (_wheelMinAngularVelocity > _wheelMaxAngularVelocity)
        //    _wheelMinAngularVelocity -= _wheelMaxAngularVelocity;

        _engine.SetWheelsRotationSpeed(_wheelMinRotationSpeed, _wheelMaxRotationSpeed);
    }
}