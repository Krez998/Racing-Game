using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class Wheel : MonoBehaviour
{
    public WheelCollider WheelCollider => _wheelCollider;

    [SerializeField] private WheelCollider _wheelCollider;
    [SerializeField] private TrailRenderer _skidTrail;
    [SerializeField] private ParticleSystem _tyreBurnoutSmoke;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] private bool _isGrounded;
    [SerializeField, Range(0, 1)] private float _skidAudioVolume;

    private AudioSource _skidAudio;
    private WheelHit _wheelHit;

    private void Awake()
    {
        _skidAudio = GetComponent<AudioSource>();
        _skidAudio.volume = 0;
        _skidAudio.Play();
        _skidAudio.Pause();
    }

    private void Update()
    {
        _wheelCollider.GetWorldPose(out Vector3 posiition, out Quaternion rotation);
        transform.position = posiition;
        transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        _isGrounded = _wheelCollider.isGrounded;
        SkidCheck();
    }

    private void SkidCheck()
    {
        _wheelCollider.GetGroundHit(out _wheelHit);

        if ((Mathf.Abs(_wheelHit.forwardSlip) > 0.9f || Mathf.Abs(_wheelHit.sidewaysSlip) > 0.4f)
            && _isGrounded && ((1 << _wheelHit.collider.gameObject.layer) & _groundLayer) != 0)
        {
            //Debug.Log("GROUNDED");
            if (_skidTrail != null)
                _skidTrail.emitting = true;

            if (_tyreBurnoutSmoke != null)
                _tyreBurnoutSmoke.Play(true);

            if (_skidAudio.volume < _skidAudioVolume)
            {
                _skidAudio.UnPause();
                _skidAudio.volume += 0.01f;
            }
        }
        else
        {
            //Debug.Log("NOT GROUNDED");
            if (_skidTrail != null)
                _skidTrail.emitting = false;

            if (_tyreBurnoutSmoke != null)
                _tyreBurnoutSmoke.Stop();

            if (_skidAudio.volume > 0)
            {               
                _skidAudio.volume -= 0.01f;
            }
            else
                _skidAudio.Pause();
        }
    }
}
