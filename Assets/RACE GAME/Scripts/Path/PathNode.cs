using UnityEngine;

public class PathNode : MonoBehaviour
{
    public float TargetSpeed => _targetSpeed;

    [SerializeField] private float _targetSpeed;
}
