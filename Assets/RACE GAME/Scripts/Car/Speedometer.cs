using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Speedometer : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] private TextMeshProUGUI _spedometerText;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float speed = (_rb.velocity.magnitude * 3.6f);
        if( _spedometerText != null )
            _spedometerText.text = Numbers.GeneratedNumsStr[(int)Mathf.Round(speed)];
    }
}
