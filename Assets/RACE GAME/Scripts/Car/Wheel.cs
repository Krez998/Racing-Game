using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Wheel : MonoBehaviour
{
    public WheelCollider WheelCollider => _wheelCollider;

    [SerializeField] private WheelCollider _wheelCollider;
    [SerializeField] private TrailRenderer _skidTrail;
    [SerializeField] private ParticleSystem _tyreBurnoutSmoke;

    [SerializeField, Range(0, 1)] private float _skidAudioVolume;

    private AudioSource _skidAudio;

    private WheelHit _wheelHit;
    private RaycastHit _hit;

    public float _sidewaysSlip;
    public float _forwardSlip;
    //public Vector3 direction;
    public bool isGrounded;

    private void Awake()
    {
        _skidAudio = GetComponent<AudioSource>();
        _skidAudio.volume = 0;
        _skidAudio.Play();
    }

    private void FixedUpdate()
    {
        _wheelCollider.GetWorldPose(out Vector3 posiition, out Quaternion rotation);
        transform.position = posiition;
        transform.rotation = rotation;

        isGrounded = _wheelCollider.isGrounded;

        Skid();
    }

    private void Skid()
    {
        _wheelCollider.GetGroundHit(out _wheelHit);

        _sidewaysSlip = _wheelHit.sidewaysSlip;
        _forwardSlip = _wheelHit.forwardSlip;

        if ((Mathf.Abs(_wheelHit.forwardSlip) > 0.9f || Mathf.Abs(_wheelHit.sidewaysSlip) > 0.5f) && isGrounded)
        {
            //Debug.Log("GROUNDED");
            if (_skidTrail)
                _skidTrail.emitting = true;

            if (_tyreBurnoutSmoke)
            {
                //_tyreBurnoutSmoke.Simulate(1);
                _tyreBurnoutSmoke.Play(true);
            }

            if (_skidAudio.volume < _skidAudioVolume)
                _skidAudio.volume += 0.01f;
        }
        else
        {
            //Debug.Log("NOT GROUNDED");
            if (_skidTrail)
                _skidTrail.emitting = false;

            if (_tyreBurnoutSmoke)
                _tyreBurnoutSmoke.Stop();

            if (_skidAudio.volume > 0)
                _skidAudio.volume -= 0.01f;
        }

        //direction = _hit.sidewaysDir;

    }
}
