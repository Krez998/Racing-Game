using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Speedometer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TextMeshProUGUI _spedometerText;
    [SerializeField] private Image _speedometerFill;
    private Rigidbody _rigidbody;

    private GearBox _gearBox;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gearBox = GetComponent<GearBox>();
    }

    private void LateUpdate()
    {
        _speed = (_rigidbody.velocity.magnitude * 3.6f);
        _spedometerText.text = Numbers.GeneratedNumsStr[(int)Mathf.Round(_speed)];

        if (_gearBox._gearBoxMode == GearBoxMode.Neutral && _gearBox.Speed == 0)
            _speedometerFill.fillAmount = 0f;
        else
            _speedometerFill.fillAmount = _gearBox.Speed / _gearBox.CurrentGearMaxSpeed;
    }
}