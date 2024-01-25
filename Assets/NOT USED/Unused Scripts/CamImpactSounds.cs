using System;
using UnityEngine;

public class CamImpactSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    //[SerializeField] private float _maxVolume;
    //[Tooltip("Значение максимальной скорости, при котором звук удара будет воспроизводиться на максимальной громкости")]
    //[SerializeField] float _maxSpeed;
    [SerializeField] private AudioClip[] _impact;
    [SerializeField] private AudioClip[] _accident;
    [SerializeField] private AudioClip[] _landing;
    [SerializeField] private LayerMask _carLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _groundLayer;

    private float _relativeVelocity;
    private float _volume;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _carLayer) != 0 || ((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            _relativeVelocity = Mathf.Abs(collision.relativeVelocity.z * 3.6f);
            _volume = (float)Math.Round(_relativeVelocity / 50  /*_maxSpeed*/, 2);

            _audioSource.volume = _volume;

            if (_relativeVelocity > 50)
            {
                _audioSource.volume = 0.5f;
                _audioSource.PlayOneShot(_accident[UnityEngine.Random.Range(0, _accident.Length)]);
            }
            else
                _audioSource.PlayOneShot(_impact[UnityEngine.Random.Range(0, _impact.Length)]);

            //Debug.Log("volume: " + _volume + " relV: " + _relativeVelocity);
        }

        if (((1 << collision.gameObject.layer) & _groundLayer) != 0)
        {
            _relativeVelocity = Mathf.Abs(collision.relativeVelocity.y * 3.6f);

            //Debug.Log("fall relV: " + _relativeVelocity);

            if (collision.relativeVelocity.y * 3.6 > 10f)
            {
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_impact[UnityEngine.Random.Range(0, _impact.Length)]);
            }
              
            if (collision.relativeVelocity.y * 3.6 > 25f)
            {
                _audioSource.volume = .25f;
                _audioSource.PlayOneShot(_landing[UnityEngine.Random.Range(0, _landing.Length)]);
            }
        }
    }
}