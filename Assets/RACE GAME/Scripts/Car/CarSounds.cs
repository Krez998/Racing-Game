using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CarEngine))]
public class CarSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _engineAudio;
    [SerializeField] private AudioSource _engineIdlingAudio;
    [SerializeField] private AudioSource _idleAudio;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    private float _pitchFromCar;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _currentSpeed;

    private Rigidbody _rb;
    private CarEngine _carEngine;
    private GearShift _gearShift;

    private void EngineSound()
    {
        ChangePitchFromCar();
        ChangeMinPicth();
        ChangeIdleVolume();

        _currentSpeed = Mathf.Round(transform.InverseTransformDirection(_rb.velocity).z * 3.6f);

        ChangeEngineVolume();

        if (_currentSpeed < _minSpeed)
        {
            //_engineIdlingAudio.pitch = _minPitch;
        }

        if (_currentSpeed > _minSpeed)
        {
            _engineAudio.pitch = _minPitch + _pitchFromCar;       
        }

        if (_currentSpeed > _maxSpeed)
        {
            //_engineIdlingAudio.pitch = _maxPitch;
        }
    }


    private void ChangeMinPicth() => _minPitch = 1.1f - 0.15f * _gearShift.CurrentGear;

    private void ChangePitchFromCar()
    {
        if (_gearShift.CurrentGearMaxSpeed != 0)
            _pitchFromCar = _currentSpeed / _gearShift.CurrentGearMaxSpeed;
    }

    private void ChangeIdleVolume()
    {
        if (_currentSpeed < 3 && _idleAudio.volume < 0.1f)
        {
            _idleAudio.volume += 0.01f;
        }
        else if (_currentSpeed > 3 && _idleAudio.volume != 0f)
            _idleAudio.volume -= 0.01f;
    }

    private void ChangeEngineVolume()
    {
        if (_carEngine.MotorTorque == 0 && _engineAudio.volume > 0.35f)
        {
            _engineAudio.volume -= 0.01f;
        }
        else if (_carEngine.MotorTorque == 0 && _engineAudio.volume > 0f && _currentSpeed < 5)
        {
            _engineAudio.volume -= 0.005f;
        }
        else if (_carEngine.MotorTorque > 0 && _engineAudio.volume < 0.5f)
            _engineAudio.volume += 0.01f;
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _carEngine = GetComponent<CarEngine>();
        _gearShift = GetComponent<GearShift>();
    }

    private void Update()
    {
        EngineSound();
    }
}
