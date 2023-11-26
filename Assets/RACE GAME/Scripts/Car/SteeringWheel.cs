using System;
using UnityEngine;

public class SteeringWheel : MonoBehaviour, ISteerable
{
    [SerializeField, Range(0, 40)] private float _maxRotationAngle;
    [SerializeField] private float _defaultSteeringAngle;

    [Header("Wheels")]
    [SerializeField] private Wheel[] _wheels = new Wheel[2];


    public void TurnLeft(float angle)
    {
        if (angle > _maxRotationAngle)
            angle = _maxRotationAngle;
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[0].WheelCollider.steerAngle, -angle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        if (angle > _maxRotationAngle)
            angle = _maxRotationAngle;
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[0].WheelCollider.steerAngle, angle, 0.1f);
    }

    public void StraightenSteeringWheel()
    {
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[0].WheelCollider.steerAngle, _defaultSteeringAngle, 0.1f);
    }

}
