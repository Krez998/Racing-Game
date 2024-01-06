using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float _angle;
    [SerializeField] private Wheel[] _wheels;

    private void FixedUpdate()
    {
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        _angle = Vector3.SignedAngle(transform.forward, targetPos - transform.position, Vector3.up);
        RotateWheelsToWaypoint(_angle);

        Debug.DrawLine(transform.position, transform.position + transform.forward * 8f, Color.green);
        Debug.DrawLine(transform.position, target.position, Color.green);
    }

    private void RotateWheelsToWaypoint(float angle)
    {
        if (angle < 0)
            TurnLeft(angle);
        else if (angle > 0)
            TurnRight(angle);
    }

    public void TurnLeft(float angle)
    {
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, angle, 0.1f);
    }

    public void TurnRight(float angle)
    {
        for (int i = 0; i < _wheels.Length; i++)
            _wheels[i].WheelCollider.steerAngle = Mathf.Lerp(_wheels[i].WheelCollider.steerAngle, angle, 0.1f);
    }
}
