using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Speedometer : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] private TextMeshProUGUI _spedometerText;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float speed = (_rigidbody.velocity.magnitude * 3.6f);
        if( _spedometerText != null )
            _spedometerText.text = Numbers.GeneratedNumsStr[(int)Mathf.Round(speed)];
    }
}
