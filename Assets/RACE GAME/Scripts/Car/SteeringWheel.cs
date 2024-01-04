using System;
using UnityEngine;

[RequireComponent(typeof(Speedometer))]
public class SteeringWheel : MonoBehaviour, ISteerable
{
    [SerializeField, Range(0, 40)] private float _maxRotationAngle;
    [SerializeField] private float _defaultSteeringAngle;
    [SerializeField] private Wheel[] _wheels = new Wheel[2];
    [SerializeField] bool _useReduceSteerangle;
    private float _reducedSteerangle;
    private Speedometer _speedometer;

    private void Awake()
    {
        _speedometer = GetComponent<Speedometer>();
    }

    public void TurnLeft(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? angle - (angle / 200f) * _speedometer.GetSpeed() : angle;

        if (_reducedSteerangle > _maxRotationAngle)
            _reducedSteerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, -_reducedSteerangle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? angle - (angle / 200f) * _speedometer.GetSpeed() : angle;

        if (_reducedSteerangle > _maxRotationAngle)
            _reducedSteerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, _reducedSteerangle, 0.1f);
    }

    public void StraightenSteeringWheel()
    {
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, _defaultSteeringAngle, 0.1f);
    }
}
