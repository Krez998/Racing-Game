using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Path Params")]
    [SerializeField] private Path _path;
    [SerializeField] private Transform _currentWaypoint;
    [SerializeField] private int _currentTargetIndex;

    [Header("Rotation")]
    public float rotationAngle;

    private IMovable _movable;
    private ISteerable _steerable;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
        FindFirstWaypoint();
    }

    private void FixedUpdate()
    {
        if (_currentWaypoint != null)
        {
            if (Vector3.Distance(transform.position, _currentWaypoint.position) < 2f)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _currentWaypoint = _path.Waypoints[_currentTargetIndex];
            }

            Vector3 targetVector = new Vector3(_currentWaypoint.position.x, transform.position.y, _currentWaypoint.position.z);
            rotationAngle = Vector3.SignedAngle(targetVector - transform.position, transform.forward, Vector3.up);
        }

        _movable.Acceleration();
        Steer(rotationAngle);
    }

    private void Steer(float angle)
    {
        if(angle > 0)
            _steerable.TurnLeft(angle);
        else if(angle < 0)
            _steerable.TurnRight(-angle);
    }

    private void FindFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {
            _currentWaypoint = _path.Waypoints[0];
            _currentTargetIndex = 0;
        }
    }

}
