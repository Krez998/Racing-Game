using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Path Params")]
    [SerializeField] private Path _path;
    [SerializeField] private PathNode _currentWaypoint;
    [SerializeField] private int _currentTargetIndex;

    [SerializeField, Tooltip("Угол поворота колеса в сторону чекпоинта")] private float _rotationAngleToPathNode;

    private IMovable _movable;
    private ISteerable _steerable;
    private IGearShift _gearShift;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
        _gearShift = GetComponent<IGearShift>();
        
        FindFirstWaypoint();
    }

    private void FindFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {
            _currentTargetIndex = 0;
            _currentWaypoint = _path.Waypoints[_currentTargetIndex];
            _gearShift.SetSpeedLimit(_currentWaypoint.TargetSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (_currentWaypoint != null)
        {
            if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 2f)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _currentWaypoint = _path.Waypoints[_currentTargetIndex];
                _gearShift.SetSpeedLimit(_currentWaypoint.TargetSpeed);
            }

            Vector3 targetVector = new Vector3(_currentWaypoint.transform.position.x, transform.position.y, _currentWaypoint.transform.position.z);
            _rotationAngleToPathNode = Vector3.SignedAngle(targetVector - transform.position, transform.forward, Vector3.up);
        }

        _movable.Acceleration();
        Steer(_rotationAngleToPathNode);
    }

    private void Steer(float angle)
    {
        if(angle > 0)
            _steerable.TurnLeft(angle);
        else if(angle < 0)
            _steerable.TurnRight(-angle);
    }

    
}
