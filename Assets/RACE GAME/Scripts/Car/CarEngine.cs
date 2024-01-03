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

    [Tooltip(" рут€щий момент")]
    [SerializeField, Range(0, 5_000)] private float _torqueForce;
    
    [Tooltip("“ормозной момент")]
    [SerializeField, Range(2500, 5000)] private float _brakeForce;

    [Tooltip("–аспределение тормозных сил между ос€ми: 0 - передн€€; 1 - задн€€")]
    [SerializeField, Range(0, 1)] private float _brakeForceAxlesRatio;

    //[SerializeField, Range(0, 50)] private float _decelerationForce;

    private GearBox _gearShift;
    private Speedometer _speedometer;
    private Vector3 _lastPosition;
    private Vector3 _differencePosition;
    private float _motorTorque; // крут€щий момент мотора
    private float _wheelTorque; // крут€щий момент на колеса
    private float _wheelMinAngularVelocity;
    public float _wheelMaxAngularVelocity;
    public float _wheelAngularVelocity;

    // временно
    //public float ROT_SPEED;
    //public float DIFF_POS_Z;
    private float _gasInput;

    private void Awake()
    {
        _gearShift = GetComponent<GearBox>();
        _speedometer = GetComponent<Speedometer>();
        Deceleration();
    }

    public void SetWheelDriveMode(WheelDriveMode wheelDriveMode)
    {
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

    private void CheckGasInput()
    {
        if (_motorTorque != 0 && _gasInput < 1f)
            _gasInput += Time.deltaTime * 0.5f;
        else if (_motorTorque == 0 && _gasInput > 0f)
            _gasInput -= Time.deltaTime * 0.1f;
    }

    public void ResetGasInput()
    {
        _gasInput = 0f;
    }

    private void Update()
    {
        //ROT_SPEED = _drivingWheels[0].WheelCollider.rotationSpeed;
        //DIFF_POS_Z = (float)Math.Round(_differencePosition.z, 2);

        fl_gas = _drivingWheels[0].WheelCollider.motorTorque;
        fr_gas = _drivingWheels[1].WheelCollider.motorTorque;

        fl_brake = _wheels[0].WheelCollider.brakeTorque;
        fr_brake = _wheels[1].WheelCollider.brakeTorque;
        rl_brake = _wheels[2].WheelCollider.brakeTorque;
        rr_brake = _wheels[3].WheelCollider.brakeTorque;

        CheckGasInput();
    }

    public void Acceleration()
    {
        //_differencePosition = transform.position - _lastPosition;
        //_differencePosition = transform.InverseTransformDirection(_differencePosition);

        //if (_differencePosition.z < -0.001)
        if (_speedometer.GetSpeed() < 0)
            Brake();
        else
        {
            _motorTorque = _torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            if (_gearShift.GearBoxMode == GearBoxMode.Forward)
                _wheelTorque = _motorTorque;
            else
                _wheelTorque = 0f;

            for (int i = 0; i < _drivingWheels.Length; i++)
            {
                _drivingWheels[i].WheelCollider.motorTorque = _wheelTorque;

                _wheelAngularVelocity = Mathf.Lerp(_wheelMinAngularVelocity, _wheelMaxAngularVelocity, _gasInput);
                _drivingWheels[i].WheelCollider.rotationSpeed = _wheelAngularVelocity;
            }
        }

        //_lastPosition = transform.position;
    }

    public void Reverse()
    {      
        //_differencePosition = transform.position - _lastPosition;
        //_differencePosition = transform.InverseTransformDirection(_differencePosition);

        //if (_differencePosition.z > 0.001f)

        if(_speedometer.GetSpeed() > 0)
            Brake();
        else
        {
            _motorTorque = -_torqueForce;

            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].WheelCollider.brakeTorque = 0;

            if (_gearShift.GearBoxMode == GearBoxMode.Backward)
                _wheelTorque = _motorTorque;
            else
                _wheelTorque = 0f;
            
            for (int i = 0; i < _drivingWheels.Length; i++)
            {
                _drivingWheels[i].WheelCollider.motorTorque = _wheelTorque;

                _wheelAngularVelocity = Mathf.Lerp(_wheelMinAngularVelocity, _wheelMaxAngularVelocity, _gasInput);
                _drivingWheels[i].WheelCollider.rotationSpeed = -_wheelAngularVelocity;
            }
        }

        //_lastPosition = transform.position;
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