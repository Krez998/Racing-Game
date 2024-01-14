using System;
using UnityEngine;

[RequireComponent(typeof(GearBox))]
public class SteeringWheel : MonoBehaviour, ISteerable
{
    [SerializeField, Range(0, 40)] private float _maxRotationAngle;
    [SerializeField] private Wheel[] _wheels = new Wheel[2];
    [SerializeField] bool _useReduceSteerangle;
    [SerializeField] private float _steerangle;
    private GearBox _gearBox;
    private float _defaultSteerangle = 0f;

    private void Awake()
    {
        _gearBox = GetComponent<GearBox>();
    }

    private void ReduceSteerangle(float angle)
    {
        _steerangle = Mathf.Abs(angle) - (Mathf.Abs(angle) / 200f) * _gearBox.Speed;
    }

    public void TurnLeft(float angle)
    {
        if (_useReduceSteerangle)
            ReduceSteerangle(angle);
        else
            _steerangle = angle;

        if (_steerangle > _maxRotationAngle)
            _steerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, -_steerangle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        if (_useReduceSteerangle)
            ReduceSteerangle(angle);
        else
            _steerangle = angle;

        if (_steerangle > _maxRotationAngle)
            _steerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, _steerangle, 0.1f);
    }

    public void StraightenSteeringWheel()
    {
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, _defaultSteerangle, 0.1f);
    }
}
