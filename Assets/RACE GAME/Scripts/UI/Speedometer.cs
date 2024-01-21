using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GearBox))]
public class Speedometer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedABS;

    private Rigidbody _rigidbody;
    private GearBox _gearBox;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gearBox = GetComponent<GearBox>();
    }

    private void FixedUpdate()
    {
        _speed = Mathf.Round(transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f);
        _speedABS = _rigidbody.velocity.magnitude * 3.6f;
    }

    private void LateUpdate()
    {
        GameEvents.OnSpeedometerUpdating?.Invoke(_speedABS);
        GameEvents.OnSpeedIndicatorUpdating?.Invoke(_speed,
            _gearBox.GearBoxMode,
            _gearBox.CurrentGearMinSpeed,
            _gearBox.CurrentGearMaxSpeed);
    }
}