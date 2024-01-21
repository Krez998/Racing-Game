using UnityEngine;

/// <summary>
/// ���������� ����� ����� ����������
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CenterOfMassGizmos : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
    }
}
