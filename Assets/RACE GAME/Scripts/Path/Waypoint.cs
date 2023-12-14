using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float TargetSpeed => _targetSpeed;

    [SerializeField] private float _targetSpeed;
}
