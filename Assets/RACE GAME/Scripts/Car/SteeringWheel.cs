using System;
using UnityEngine;

public class SteeringWheel : MonoBehaviour, ISteerable
{
    [SerializeField, Range(0, 40)] private float _maxRotationAngle;
    [SerializeField] private float _defaultSteeringAngle;
    [SerializeField] private Wheel[] _wheels = new Wheel[2];
    [SerializeField] bool _useReduceSteerangle;
    private float _reducedSteerangle;
    private IGearBox _gearBox;

    private void Awake()
    {
        _gearBox = GetComponent<IGearBox>();
    }

    public void TurnLeft(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? angle - (angle / 200f) * _gearBox.GetSpeed() : angle;

        if (_reducedSteerangle > _maxRotationAngle)
            _reducedSteerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, -_reducedSteerangle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? angle - (angle / 200f) * _gearBox.GetSpeed() : angle;

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
