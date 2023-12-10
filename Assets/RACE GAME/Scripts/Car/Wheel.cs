using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Wheel : MonoBehaviour
{
    public WheelCollider WheelCollider => _wheelCollider;

    [SerializeField] private WheelCollider _wheelCollider;
    [SerializeField] private TrailRenderer _skidTrail;

    [SerializeField, Range(0, 1)] private float _skidAudioVolume;

    private AudioSource _skidAudio;

    private WheelHit _hit;
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

    private void LateUpdate()
    {
        _wheelCollider.GetWorldPose(out Vector3 posiition, out Quaternion rotation);
        transform.position = posiition;
        transform.rotation = rotation;


        isGrounded = _wheelCollider.isGrounded;

        Skid();
    }

    private void Skid()
    {
        _wheelCollider.GetGroundHit(out _hit);

        if (Mathf.Abs(_hit.forwardSlip) > 0.8f || Mathf.Abs(_hit.sidewaysSlip) > 0.4f && _wheelCollider.isGrounded)
        {
            Debug.Log("GROUNDED");
            if (_skidTrail)
                _skidTrail.emitting = true;

            if(_skidAudio.volume < _skidAudioVolume)
            _skidAudio.volume += 0.01f;
        }
        else
        {
            Debug.Log("NOT GROUNDED");
            if (_skidTrail)
                _skidTrail.emitting = false;

            if (_skidAudio.volume > 0)
                _skidAudio.volume -= 0.01f;
        }


        _sidewaysSlip = _hit.sidewaysSlip;
        _forwardSlip = _hit.forwardSlip;
        //direction = _hit.sidewaysDir;

    }
}
