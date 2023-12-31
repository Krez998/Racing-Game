using TMPro;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Car _targetCar;

    [SerializeField] private float _cameraHeight;
    [SerializeField] private float _cameraForward;
    [SerializeField] private float _speed;
    [SerializeField] private float _angleSpeed;

    private Vector3 _offset;

    public void SetTarget(Car car)
    {
        _targetCar = car;
    }

    private void Start()
    {
        _offset = new Vector3(0, _cameraHeight, _cameraForward);
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _targetCar.CameraTarget.TransformPoint(_offset);

        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed);
        transform.LookAt(_targetCar.CameraTarget.position);
    }
}