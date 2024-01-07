using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float TargetSpeed => _targetSpeed;

    [SerializeField] private float _targetSpeed;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
        //Handles.DrawWireDisc(transform.position, transform.up, 8f);
    }
}
