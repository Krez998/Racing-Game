using System;
using UnityEngine;

public enum WheelDriveMode
{
    RWD,
    FWD,
    AWD
}

[RequireComponent(typeof(GearBox))]
[RequireComponent(typeof(Speedometer))]
public class CarEngine : MonoBehaviour, IMovable
{
    public float MotorTorque => _motorTorque;
    public Wheel[] DrivingWheels => _drivingWheels;

    [SerializeField] private WheelDriveMode _wheelDriveMode;

    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private Wheel[] _drivingWheels;

    [Tooltip(" рут€щий момент")]
    [SerializeField, Range(0, 5_000)] private float _torqueForce;
    
    [Tooltip("“ормозной момент")]
    [SerializeField, Range(2500, 5000)] private float _brakeForce;

    [Tooltip("–аспределение тормозных сил между ос€ми: 0 - передн€€; 1 - задн€€")]
    [SerializeField, Range(0, 1)] private float _brakeForceAxlesRatio;

    //[SerializeField, Range(0, 50)] private float _decelerationForce;

    private GearBox _gearShift;
    private Speedometer _speedometer;

    //private float _motorTorque; // крут€щий момент мотора
    public float _motorTorque; // крут€щий момент на колеса
    public float _wheelMinAngularVelocity;
    public float _wheelMaxAngularVelocity;
    public float _wheelAngularVelocity;

    public float _gasInput;

    public void GetData(float motorTorque, float brakeTorque, WheelDriveMode wheelDriveMode)
    {
        _torqueForce = motorTorque;
        _brakeForce = brakeTorque;
        _wheelDriveMode = wheelDriveMode;

        switch (_wheelDriveMode)
        {
            case WheelDriveMode.FWD:
                _drivingWheels = new Wheel[2];
                for (int i = 0; i < _drivingWheels.Length; i++)
                    _drivingWheels[i] = _wheels[i];
                break;
            case WheelDriveMode.RWD:
                _drivingWheels = new Wheel[2];
                for (int i = 0; i < _drivingWheels.Length; i++)
                    _drivingWheels[i] = _wheels[i + 2];
                break;
            case WheelDriveMode.AWD:
                _drivingWheels = new Wheel[4];
                for (int i = 0; i < _drivingWheels.Length; i++)
                    _drivingWheels[i] = _wheels[i];
                break;
        }
    }

    public void SetWheelAngularVelocity(float wheelMinAngularVelocity, float wheelMaxAngularVelocity)
    {
        _wheelMinAngularVelocity = wheelMinAngularVelocity;
        _wheelMaxAngularVelocity = wheelMaxAngularVelocity;
    }

    public void ResetGasInput()
    {
        _gasInput = 0f;
    }

    private void Awake()
    {
        _gearShift = GetComponent<GearBox>();
        _speedometer = GetComponent<Speedometer>();
    }

    private void CheckGasInput()
    {
        if (_motorTorque != 0 && _gasInput < 1f)
            _gasInput += 0.01f;
        else if (_motorTorque == 0 && _gasInput > 0f)
            _gasInput -= 0.005f;
    }

    private void FixedUpdate()
    {
        CheckGasInput();
    }

    public void Acceleration()
    {
        if (_speedometer.GetSpeed() != 0 && !_speedometer.MovesInForwardDirection)
            Brake();
        else
        {
            _motorTorque = _torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            if (_gearShift.GearBoxMode == GearBoxMode.Forward)
            {
                for (int i = 0; i < _drivingWheels.Length; i++)
                {
                    _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

                    _wheelAngularVelocity = Mathf.Lerp(_wheelMinAngularVelocity, _wheelMaxAngularVelocity, _gasInput);
                    _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngularVelocity;
                }
            }
        }
    }

    public void Reverse()
    {      
        if(_speedometer.GetSpeed() != 0 && _speedometer.MovesInForwardDirection)
            Brake();
        else
        {
            _motorTorque = -_torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;
           
            if (_gearShift.GearBoxMode == GearBoxMode.Backward)
            {
                for (int i = 0; i < _drivingWheels.Length; i++)
                {
                    _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

                    _wheelAngularVelocity = Mathf.Lerp(_wheelMinAngularVelocity, _wheelMaxAngularVelocity, _gasInput);
                    _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngularVelocity;
                }
            }
        }
    }

    public void Brake()
    {
        //Debug.Log("Brake");
        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length - 2; i++)
            _wheels[i].WheelCollider.brakeTorque = _brakeForce * (1 - _brakeForceAxlesRatio);

        for (int i = 2; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = _brakeForce * _brakeForceAxlesRatio;
    }

    public void Deceleration()
    {
        //Debug.Log("Deceleration");
        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0f;
            //_wheels[i].WheelCollider.brakeTorque = _decelerationForce;
    }
}