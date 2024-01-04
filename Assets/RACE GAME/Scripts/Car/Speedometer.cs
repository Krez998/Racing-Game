using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GearBox))]
public class Speedometer : MonoBehaviour, ISpeedometer
{
    public bool MovesInForwardDirection => _movesInForwardDirection;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedABS;
    [SerializeField] private TextMeshProUGUI _spedometerText;
    [SerializeField] private Image _speedometerFill;
    private Rigidbody _rigidbody;
    private GearBox _gearBox;

    public bool _movesInForwardDirection;

    public float GetSpeed()
    {
        return _speed;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gearBox = GetComponent<GearBox>();
    }

    private void FixedUpdate()
    {     
        _speed = Mathf.Round(transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f);
        _speedABS = _rigidbody.velocity.magnitude * 3.6f;
        _movesInForwardDirection = _speed > 0 ? true : false;   
    }

    private void LateUpdate()
    {
        if (_spedometerText)
        {
            if (_speed > 0)
                _spedometerText.text = Numbers.CachedNums[(int)Mathf.Round(_speed)];
            else
                _spedometerText.text = Numbers.CachedNums[(int)Mathf.Round(-_speed)];
        }

        if (_speedometerFill)
        {
            if (_gearBox.GearBoxMode == GearBoxMode.Neutral && _speed == 0)
            {
                _speedometerFill.fillAmount = 0f;
            }
            else
            {
                //_speedometerFill.fillAmount = _speed / _gearBox.CurrentGearMaxSpeed;

                var value = _gearBox.CurrentGearMinSpeed;
                var amount = (_speedABS - value) / (Mathf.Abs(_gearBox.CurrentGearMaxSpeed) - value);
                //Debug.Log((_speed - value) + "/" + (_gearBox.CurrentGearMaxSpeed - value));
                _speedometerFill.fillAmount = amount;
            }
        }
    }
}