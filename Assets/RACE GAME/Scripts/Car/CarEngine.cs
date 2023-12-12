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

    [Header("All Wheels")]
    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private Wheel[] _drivingWheels;

    // временно
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

    private GearBox _gearShift;
    private Vector3 _lastPosition;
    private Vector3 _differencePosition;

    private float _motorTorque; // крутящий момент мотора
    private float _wheelTorque; // крутящий момент на колеса
    private float _wheelAngularVelocity;

    public bool IsAllowMove { set => _isAllowMove = value; }
    public bool _isAllowMove;

    private void Awake()
    {
        _gearShift = GetComponent<GearBox>();
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

    public void SetWheelAngularVelocity(float velocity) => _wheelAngularVelocity = velocity;

    private void FixedUpdate()
    {
        fl_gas = _drivingWheels[0].WheelCollider.motorTorque;
        fr_gas = _drivingWheels[1].WheelCollider.motorTorque;

        fl_brake = _wheels[0].WheelCollider.brakeTorque;
        fr_brake = _wheels[1].WheelCollider.brakeTorque;
        rl_brake = _wheels[2].WheelCollider.brakeTorque;
        rr_brake = _wheels[3].WheelCollider.brakeTorque;

        _gearShift.GetEngineData(_motorTorque);
    }

    public void Acceleration()
    {
        _motorTorque = _torqueForce;

        _differencePosition = transform.position - _lastPosition;
        _differencePosition = transform.InverseTransformDirection(_differencePosition);

        //if (_differencePosition.z > 0.01f)
        //    Brake();
        //else
        //{
            if (_isAllowMove)
                _wheelTorque = _motorTorque;
            else
                _wheelTorque = 0f;


            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            for (int i = 0; i < _drivingWheels.Length; i++)
            {
                _drivingWheels[i].WheelCollider.motorTorque = _wheelTorque;
                _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngularVelocity;
            }
        //}
    }

    public void Reverse()
    {
        _motorTorque = -_torqueForce;

        _differencePosition = transform.position - _lastPosition;
        _differencePosition = transform.InverseTransformDirection(_differencePosition);

        if (_differencePosition.z > 0.01f)
            Brake();
        else
        {           
            if (_isAllowMove)
                _wheelTorque = _motorTorque;
            else
                _wheelTorque = 0f;

            
            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            for (int i = 0; i < _drivingWheels.Length; i++)
            {
                _drivingWheels[i].WheelCollider.motorTorque = _wheelTorque;
                _drivingWheels[i].WheelCollider.rotationSpeed = -_wheelAngularVelocity;
            }
        }

        _lastPosition = transform.position;
    }

    public void Deceleration()
    {
        //Debug.Log("Deceleration");

        _motorTorque = 0f;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.motorTorque = _motorTorque;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.brakeTorque = _decelerationForce;

        for (int i = 0; i < _drivingWheels.Length; i++)
            _drivingWheels[i].WheelCollider.brakeTorque = _decelerationForce;
    }

    public void Brake()
    {
        //Debug.Log("Brake");

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