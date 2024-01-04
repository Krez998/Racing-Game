using UnityEngine;

/// <summary>
/// ������ ���������� �� ��� X
/// </summary>
public class CarTiltX : MonoBehaviour
{
    [SerializeField] private float _tiltX;

    private void Update()
    {
        CalculateCarTiltX();
    }

    private void CalculateCarTiltX()
    {
        Vector3 verticVector = Vector3.up;
        _tiltX = Vector3.Angle(verticVector, transform.forward);
    }
}
