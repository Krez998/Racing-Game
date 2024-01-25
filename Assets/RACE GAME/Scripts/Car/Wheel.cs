using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelCollider WheelCollider => _wheelCollider;
    [SerializeField] private WheelCollider _wheelCollider;

    private void Update()
    {
        _wheelCollider.GetWorldPose(out Vector3 posiition, out Quaternion rotation);
        transform.position = posiition;
        transform.rotation = rotation;
    }
}
