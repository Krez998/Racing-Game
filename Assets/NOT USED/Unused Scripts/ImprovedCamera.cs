using UnityEngine;
using Zenject;

public class ImproverCamera : MonoBehaviour
{
    [SerializeField] private Car _targetCar;
    
    [SerializeField] private float _cameraHeight;

    [SerializeField] private float _targerDistToFollow = 3f;
    [SerializeField] private float _speedModify = 0.001f;

    private Rigidbody _rigidbody;
    private Vector3 _offset;
    private Vector3 targetPosition;

    //[Inject]
    //private void Construct(Car car)
    //{
    //    Debug.Log(car.GetType());
    //    _targetCar = car;
    //}

    private void Start()
    {
        SetCameraOffset();
        FindPlayerCar();
    }

    private void FindPlayerCar()
    {
        _targetCar = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        _rigidbody = _targetCar.GetComponent<Rigidbody>();
    }

    private void SetCameraOffset() => _offset = new Vector3(0, _cameraHeight, 0);

    private void FixedUpdate()
    {
        _offset = new Vector3(0, _cameraHeight, 0);

        targetPosition = _targetCar.CameraTarget.TransformPoint(_offset);

        transform.LookAt(_targetCar.CameraTarget.position);

        if (Vector3.Distance(transform.position, _targetCar.CameraTarget.position) > _targerDistToFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, _rigidbody.velocity.magnitude * 3.6f * _speedModify);
        }
    }
}
