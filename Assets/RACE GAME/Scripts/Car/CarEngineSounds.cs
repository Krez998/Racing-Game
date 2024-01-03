using UnityEngine;

[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(GearBox))]
[RequireComponent(typeof(Speedometer))]
public class CarEngineSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _accelerationAudio;
    [SerializeField] private AudioSource _decelerationAudio;
    [SerializeField] private AudioSource _idleAudio;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _currentSpeed;

    private CarEngine _carEngine;
    private GearBox _gearBox;
    private Speedometer _speedometer;

    public float idlingPitch;

    public void SetEngineAudioclips(AudioClip acceleration, AudioClip deceleration, AudioClip idle)
    {
        _accelerationAudio.clip = acceleration;
        _decelerationAudio.clip = deceleration;
        _idleAudio.clip = idle;
    }

    private void Awake()
    {
        _carEngine = GetComponent<CarEngine>();
        _gearBox = GetComponent<GearBox>();
        _speedometer = GetComponent<Speedometer>();
    }

    private void Update()
    {
        SoundEngine();
    }

    private void SoundEngine()
    {
        ChangePitchFromCar();
        ChangeMinPicth();
        ChangeIdleVolume();

        _currentSpeed = _speedometer.GetSpeed();

        ChangeAccelerationVolume();

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

        ChangePitchDecelerationAudio();
        ChangeDecelerationVolume();
    }

    private void ChangePitchDecelerationAudio()
    {
        if (_gearBox.CurrentGearMaxSpeed != 0)
            _decelerationAudio.pitch = idlingPitch + _currentSpeed / _gearBox.CurrentGearMaxSpeed;

        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) == 0)
            _decelerationAudio.pitch = 0.5f;
    }

    private void ChangeMinPicth()
    {
        if (_gearBox.CurrentGear != 0)
            _minPitch = 1.1f - 0.15f * _gearBox.CurrentGear;
    }

    private void ChangePitchFromCar()
    {
        if (_gearBox.CurrentGearMaxSpeed != 0)
            _accelerationAudio.pitch = _minPitch + _currentSpeed / _gearBox.CurrentGearMaxSpeed;

        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) == 0)
            _accelerationAudio.pitch = 0.5f;
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

    private void ChangeDecelerationVolume()
    {
        if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) < 10 && _decelerationAudio.volume > 0.1f)
        {
            _decelerationAudio.volume -= 0.05f;
        }
        else if (_carEngine.MotorTorque > 0 && Mathf.Abs(_currentSpeed) > 10 && _decelerationAudio.volume != 0.1f)
        {
            _decelerationAudio.volume = 0.15f;
        }
        else if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) > 10 && _decelerationAudio.volume < 0.25f)
        {
            _decelerationAudio.volume += 0.05f;
        }
    }

    private void ChangeAccelerationVolume()
    {
        if (_carEngine.MotorTorque == 0 && _accelerationAudio.volume > 0f)
        {
            _accelerationAudio.volume -= 0.1f;
        }
        //if (_carEngine.MotorTorque == 0 && _engineAudio.volume > 0.35f)
        //{
        //    Debug.Log("условие 1");
        //    _engineAudio.volume -= 0.01f;
        //}
        else if (_carEngine.MotorTorque == 0 && Mathf.Abs(_currentSpeed) < 5)
        {
            _accelerationAudio.volume -= 0.1f;
        }
        else if (_carEngine.MotorTorque != 0 && _accelerationAudio.volume < 0.5f && _gearBox.CurrentGear != 0)
        {
            _accelerationAudio.volume += 0.05f;
        }
        else if (_gearBox.CurrentGear == 0)
        {
            if (_accelerationAudio.volume > 0f)
                _accelerationAudio.volume -= 0.1f;
            //    _gearAudio.Play();
        }
    }   
}
