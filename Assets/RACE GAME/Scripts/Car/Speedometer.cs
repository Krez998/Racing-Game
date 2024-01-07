using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GearBox))]
public class Speedometer : MonoBehaviour, ISpeedometer
{
    public bool MovesInForwardDirection => _movesInForwardDirection;

    [SerializeField] private float _speed;
    [SerializeField] private float _speedABS;
    [SerializeField] private Image _speedometerFill;
    private Rigidbody _rigidbody;
    private GearBox _gearBox;
    private bool _movesInForwardDirection;
    private bool _isPlayerCar;

    public void GetData(bool isPlayerCar)
    {
        _isPlayerCar = isPlayerCar;
    }

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
        if (_isPlayerCar)
        {
            GameEvents.OnSpeedometerUpdating?.Invoke(_speedABS);
            GameEvents.OnSpeedIndicatorUpdating?.Invoke(
                _speed,
                _gearBox.GearBoxMode, 
                _gearBox.CurrentGearMinSpeed,
                _gearBox.CurrentGearMaxSpeed);
        }
    }
}