using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(GearBox))]
[RequireComponent(typeof(CarEngineSounds))]
[RequireComponent(typeof(CarProgress))]
public class Car : MonoBehaviour
{
    public bool IsPlayerCar => _isPlayerCar;
    public CarData CarData => _carData;
    public Transform CameraTarget => _cameraTarget;

    [SerializeField] private bool _isPlayerCar;
    [SerializeField] private CarData _carData;
    [SerializeField] private Transform _cameraTarget;

    private Rigidbody _rigidbody;
    private CarEngine _carEngine;
    private GearBox _gearBox;
    private CarEngineSounds _engineSounds;
    private CarProgress _carProgress;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carEngine = GetComponent<CarEngine>();
        _gearBox = GetComponent<GearBox>();
        _engineSounds = GetComponent<CarEngineSounds>();
        _carProgress = GetComponent<CarProgress>();

        _rigidbody.mass = _carData.Mass;

        _carEngine.GetData(_carData.MotorTorque, _carData.BrakeTorque, _carData.WheelDriveMode);
        _gearBox.GetData(_isPlayerCar, _carData.Speed, _carData.NumberOfGears);
        _engineSounds.GetData(_carData.Acceleration, _carData.Deceleration, _carData.Idle);
        _carProgress.GetData(_isPlayerCar);
    }
}