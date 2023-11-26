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

    [SerializeField, Range(0, 200)] private float _speedKMH;
    public float _speedMS;
    public float _angleSpeedRad;
    public float _wheelAngleSpeed;

    public float fl_torq;
    public float fr_torq;

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
            case WheelDriveMode.RWD:
                _drivingWheels = new Wheel[2];
                for (int i = 0; i < _drivingWheels.Length; i++)
                    _drivingWheels[i] = _wheels[i];
                break;
            case WheelDriveMode.FWD:
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
        _speedMS = _speedKMH * 1000 / 3600;
        _angleSpeedRad = _speedMS / _drivingWheels[0].WheelCollider.radius;
        _wheelAngleSpeed = _angleSpeedRad * Mathf.Rad2Deg;

        fl_torq = _drivingWheels[0].WheelCollider.motorTorque;
        fr_torq = _drivingWheels[1].WheelCollider.motorTorque;
    }

    public void Acceleration()
    {
        _motorTorque = _torqueForce;
        //_motorTorque = 1f;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0;

        for (int i = 0; i < _drivingWheels.Length; i++)
            //_drivingWheels[i].WheelCollider.motorTorque = _motorTorque;
            _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngleSpeed;
    }

    public void Reverse()
    {
        _motorTorque = -_torqueForce / 2;
        //_motorTorque = -1f;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;
            //_drivingWheels[i].WheelCollider.rotationSpeed = -_wheelAngleSpeed / 2;
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