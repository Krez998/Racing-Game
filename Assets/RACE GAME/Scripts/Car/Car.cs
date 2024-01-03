using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(CarEngineSounds))]
public class Car : MonoBehaviour
{
    public CarData CarData => _carData;
    public Transform CameraPointOfView => _cameraPointOfView;

    [SerializeField] private bool _isPlayerCar;
    [SerializeField] private CarData _carData;
    [SerializeField] private Transform _cameraPointOfView;

    private Rigidbody _rigidbody;
    private CarEngine _carEngine;
    private CarEngineSounds _engineSounds;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carEngine = GetComponent<CarEngine>();
        _engineSounds = GetComponent<CarEngineSounds>();

        _rigidbody.mass = _carData.Mass;
        _carEngine.SetWheelDriveMode(_carData.WheelDriveMode);
        _engineSounds.SetEngineAudioclips(_carData.Acceleration, _carData.Deceleration, _carData.Idle);

        if (_isPlayerCar)
        {
            SetCameraTargetOffset();
            SetCameraTarget();
        }
    }

    private void SetCameraTargetOffset()
    {
        _cameraPointOfView.transform.position = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);
    }

    private void SetCameraTarget()
    {
        var camera = FindObjectOfType<ThirdPersonCamera>();
        if (camera != null)
        {
            camera.SetTarget(this);
        }
    }


    /// <summary>
    /// Наклон автомобиля по оси X
    /// </summary>
    //private void CalculateCarTiltX()
    //{
    //    Vector3 verticVector = Vector3.up;
    //    _tiltX = Vector3.Angle(verticVector, transform.forward);
    //}
}
