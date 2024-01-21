using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(GearBox))]
[RequireComponent(typeof(CarEngineSounds))]
[RequireComponent(typeof(CarProgress))]
public class Car : MonoBehaviour
{
    public Transform CameraTarget => _cameraTarget;

    [SerializeField] private bool _isPlayerCar;
    [SerializeField] private Transform _cameraTarget;
   
    public int TargetRaiting => _targetRaiting;
    public Sprite Image => _image;
    [Header("Car Data")]
    [SerializeField] private int _targetRaiting;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _name;
    [SerializeField] private float _mass;
    [SerializeField] private WheelDriveMode _wheelDriveMode;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _brakeTorque;
    [SerializeField] private AudioClip _acceleration;
    [SerializeField] private AudioClip _deceleration;
    [SerializeField] private AudioClip _idle;

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

        _rigidbody.mass = _mass;

        _carEngine.GetData(_motorTorque, _brakeTorque, _wheelDriveMode);
        _gearBox.GetData(_isPlayerCar, _speed, _numberOfGears);
        _engineSounds.GetData(_acceleration, _deceleration, _idle);
        _carProgress.GetData(_isPlayerCar);
    }
}