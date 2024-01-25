using System;
using UnityEngine;

public enum WheelDriveMode
{
    RWD,
    FWD,
    AWD
}

[RequireComponent(typeof(GearBox))]
public class CarEngine : MonoBehaviour, IMovable
{
    public float MotorTorque => _motorTorque;
    public Wheel[] DrivingWheels => _drivingWheels;

    [SerializeField] private WheelDriveMode _wheelDriveMode;

    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private Wheel[] _drivingWheels;

    [SerializeField, Range(0, 5_000)] private float _torqueForce;
    [SerializeField, Range(2500, 5000)] private float _brakeForce;
    [SerializeField, Range(0, 1)] private float _brakeForceAxlesRatio;

    private GearBox _gearBox;

    public float _motorTorque;
    public float _wheelMinRotationSpeed;
    public float _wheelMaxRotationSpeed;
    public float _wheelRotationSpeed;

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

    public void SetWheelsRotationSpeed(float wheelMinRotationSpeed, float wheelMaxRotationSpeed)
    {
        _wheelMinRotationSpeed = wheelMinRotationSpeed;
        _wheelMaxRotationSpeed = wheelMaxRotationSpeed;
    }

    public void ResetGasInput()
    {
        _gasInput = 0f;
    }

    private void Awake()
    {
        _gearBox = GetComponent<GearBox>();
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
        if (_gearBox.Speed != 0 && !_gearBox.IsMovingInForwardDirection)
            Brake();
        else
        {
            _motorTorque = _torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            if (_gearBox.GearBoxMode == GearBoxMode.Forward)
            {
                for (int i = 0; i < _drivingWheels.Length; i++)
                {
                    _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

                    _wheelRotationSpeed = Mathf.Lerp(_wheelMinRotationSpeed, _wheelMaxRotationSpeed, _gasInput);
                    _drivingWheels[i].WheelCollider.rotationSpeed = _wheelRotationSpeed;
                }
            }
        }
    }

    public void Reverse()
    {      
        if(_gearBox.Speed != 0 && _gearBox.IsMovingInForwardDirection)
            Brake();
        else
        {
            _motorTorque = -_torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;
           
            if (_gearBox.GearBoxMode == GearBoxMode.Backward)
            {
                for (int i = 0; i < _drivingWheels.Length; i++)
                {
                    _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

                    _wheelRotationSpeed = Mathf.Lerp(_wheelMinRotationSpeed, _wheelMaxRotationSpeed, _gasInput);
                    _drivingWheels[i].WheelCollider.rotationSpeed = _wheelRotationSpeed;
                }
            }
        }
    }

    public void Brake()
    {
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
        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = 0f;
    }
}