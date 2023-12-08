using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CarEngine))]
public class CarSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _engineAudio;
    [SerializeField] private AudioSource _engineIdlingAudio;
    [SerializeField] private AudioSource _idleAudio;
    [SerializeField] private AudioSource _gearAudio;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _currentSpeed;

    private Rigidbody _rb;
    private CarEngine _carEngine;
    private GearShift _gearShift;

    public float idlingPitch;

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

        if (Mathf.Abs(_currentSpeed) > _minSpeed)
        {
            //_engineAudio.pitch = _minPitch + _pitchFromCar;
        }

        if (_currentSpeed > _maxSpeed)
        {
            //_engineIdlingAudio.pitch = _maxPitch;
        }

        ChangePicthIdlingAudio();
        ChangeIdlingVolume();
    }


    private void ChangePicthIdlingAudio()
    {
        if (_gearShift.CurrentGearMaxSpeed != 0)
            _engineIdlingAudio.pitch = idlingPitch + _currentSpeed / _gearShift.CurrentGearMaxSpeed;

        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) == 0)
            _engineIdlingAudio.pitch = 0.5f;
    }

    private void ChangeMinPicth()
    {
        if (_gearShift.CurrentGear != 0)
            _minPitch = 1.1f - 0.15f * _gearShift.CurrentGear;
    }

    private void ChangePitchFromCar()
    {
        if (_gearShift.CurrentGearMaxSpeed != 0)
            _engineAudio.pitch = _minPitch + _currentSpeed / _gearShift.CurrentGearMaxSpeed;

        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) == 0)
            _engineAudio.pitch = 0.5f;
    }

    private void ChangeIdleVolume()
    {
        if (_currentSpeed < 10 && _idleAudio.volume < 0.1f)
        {
            _idleAudio.volume += 0.01f;
        }
        else if (_currentSpeed > 10 && _idleAudio.volume != 0f)
            _idleAudio.volume -= 0.01f;
    }

    private void ChangeIdlingVolume()
    {
        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) < 10 && _engineIdlingAudio.volume > 0.1f)
        {
            _engineIdlingAudio.volume -= 0.05f;
        }
        else if (_carEngine.MotorTorque > 0 && Mathf.Abs(_currentSpeed) > 10 && _engineIdlingAudio.volume != 0.1f)
        {
            _engineIdlingAudio.volume = 0.15f;
        }
        else if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) > 10 && _engineIdlingAudio.volume < 0.25f)
        {
            _engineIdlingAudio.volume += 0.05f;
        }
    }

    private void ChangeEngineVolume()
    {
        if (_carEngine.MotorTorque == 0 && _engineAudio.volume > 0f)
        {
            _engineAudio.volume -= 0.1f;
        }
        //if (_carEngine.MotorTorque == 0 && _engineAudio.volume > 0.35f)
        //{
        //    Debug.Log("условие 1");
        //    _engineAudio.volume -= 0.01f;
        //}
        else if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) < 5)
        {
            _engineAudio.volume -= 0.1f;
        }
        else if (_carEngine.MotorTorque != 0 && _engineAudio.volume < 0.5f && _gearShift.CurrentGear != 0)
        {
            _engineAudio.volume += 0.05f;
        }
        else if (_gearShift.CurrentGear == 0)
        {
            if (_engineAudio.volume > 0f)
                _engineAudio.volume -= 0.1f;
            //    _gearAudio.Play();
        }
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
