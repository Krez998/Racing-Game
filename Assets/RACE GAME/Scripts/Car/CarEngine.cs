using System;
using UnityEngine;

public enum WheelDriveMode
{
    RWD,
    FWD,
    AWD
}

public class CarEngine : MonoBehaviour, IMovable
{
    [SerializeField] private WheelDriveMode _wheelDriveMode;

    [Header("Wheels")]
    [SerializeField] private Wheel[] _wheels;
    private Wheel[] _drivingWheels;

    [Header("Speed Km/h")]
    [SerializeField, Range(0, 200)] private float _speedKPH;

    // временно
    [Header("Speed Params (Very Demo)")]
    public float _speedMPS;
    public float _angleSpeedRad;
    public float _wheelAngleSpeed; 
    [Header("Gases (Very Demo)")]
    public float fl_gas;
    public float fr_gas;
    [Header("Brakes (Very Demo)")]
    public float fl_brake;
    public float fr_brake;
    public float rl_brake;
    public float rr_brake;


    [SerializeField, Range(0, 5_000)] private float _torqueForce;
    [SerializeField, Range(0, 50)] private float _decelerationForce;
    [SerializeField, Range(0, 15_000)] private float _brakeForce;

    private float _motorTorque;

    private void Awake()
    {
        ConfigureDriveMode();
        Deceleration();
    }

    private void ConfigureDriveMode()
    {
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


    private void FixedUpdate()
    {
        _speedMPS = _speedKPH * 1000 / 3600;
        _angleSpeedRad = _speedMPS / _drivingWheels[0].WheelCollider.radius;
        _wheelAngleSpeed = _angleSpeedRad * Mathf.Rad2Deg;

        fl_gas = _drivingWheels[0].WheelCollider.motorTorque;
        fr_gas = _drivingWheels[1].WheelCollider.motorTorque;

        fl_brake = _wheels[0].WheelCollider.brakeTorque;
        fr_brake = _wheels[1].WheelCollider.brakeTorque;
        rl_brake = _wheels[2].WheelCollider.brakeTorque;
        rr_brake = _wheels[3].WheelCollider.brakeTorque;
    }

    public void Acceleration()
    {
        _motorTorque = _torqueForce;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0;

        for (int i = 0; i < _drivingWheels.Length; i++)
        {
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;
            _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngleSpeed;
        }
    }

    public void Reverse()
    {
        _motorTorque = -_torqueForce;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0;

        for (int i = 0; i < _drivingWheels.Length; i++)
        {
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;
            _drivingWheels[i].WheelCollider.rotationSpeed = -_wheelAngleSpeed;
        }
    }

    public void Deceleration()
    {
        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = _decelerationForce;
    }

    public void Brake()
    {
        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = _brakeForce;
    }

    public void SetRWDWheelDriveMode()
    {
        _wheelDriveMode = WheelDriveMode.RWD;
        ConfigureDriveMode();
    }

    public void SetFWDWheelDriveMode()
    {
        _wheelDriveMode = WheelDriveMode.FWD;
        ConfigureDriveMode();
    }

    public void SetAWDWheelDriveMode()
    {
        _wheelDriveMode = WheelDriveMode.AWD;
        ConfigureDriveMode();
    }
}