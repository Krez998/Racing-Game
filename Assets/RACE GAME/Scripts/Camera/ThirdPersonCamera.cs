using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Car _targetCar;

    [SerializeField] private float _cameraHeight;
    [SerializeField] private float _cameraRange;
    [SerializeField] private float _speed;

    private Vector3 _offset;


    private void Start()
    {
        SetCameraOffset();
        FindPlayerCar();
    }

    private void SetCameraOffset() => _offset = new Vector3(0, _cameraHeight, _cameraRange);

    private void FindPlayerCar()
    {
        _targetCar = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _targetCar.CameraTarget.TransformPoint(_offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed);
        transform.LookAt(_targetCar.CameraTarget.position);
    }

    private void OnDrawGizmos()
    {
        if (_targetCar != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _targetCar.CameraTarget.position);
        }
    }
}