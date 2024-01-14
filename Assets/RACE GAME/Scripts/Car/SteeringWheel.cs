using System;
using UnityEngine;

[RequireComponent(typeof(GearBox))]
public class SteeringWheel : MonoBehaviour, ISteerable
{
    [SerializeField, Range(0, 40)] private float _maxRotationAngle;
    [SerializeField] private Wheel[] _wheels = new Wheel[2];
    [SerializeField] bool _useReduceSteerangle;
    private float _defaultSteeringAngle = 0f;
    private float _reducedSteerangle;
    private GearBox _gearBox;

    private void Awake()
    {
        _gearBox = GetComponent<GearBox>();
    }

    public void TurnLeft(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? Mathf.Abs(angle) - (Mathf.Abs(angle) / 200f) * _gearBox.Speed : Mathf.Abs(angle);
        
        if (_reducedSteerangle > _maxRotationAngle)
            _reducedSteerangle = _maxRotationAngle;

        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, -_reducedSteerangle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        _reducedSteerangle = _useReduceSteerangle ? angle - (angle / 200f) * _gearBox.Speed : angle;

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
