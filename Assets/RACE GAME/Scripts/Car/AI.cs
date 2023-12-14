using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Path Params")]
    [SerializeField] private AIPath _path;
    [SerializeField] private Waypoint _currentWaypoint;
    [SerializeField] private int _currentTargetIndex;
    [SerializeField, Tooltip("Угол поворота колеса в сторону чекпоинта")] private float _rotationAngleToPathNode;
    [SerializeField, Tooltip("Ограничение скорости")] private float _speedLimit = 9999f;

    private IMovable _movable;
    private ISteerable _steerable;
    private IGearBox _gearBox;
    private float _speed;
    private float _distanceToWaypoint;
    private Vector3 _vectorToTarget;
    [SerializeField] private float _waypointRange = 2f;
    public float _jamTime;
    public bool _isStucked;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
        _gearBox = GetComponent<IGearBox>();
        
        FindFirstWaypoint();
    }

    private void FindFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {
            _currentTargetIndex = 0;
            _currentWaypoint = _path.Waypoints[_currentTargetIndex];
            SetSpeedLimit(_currentWaypoint.TargetSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (_currentWaypoint != null)
        {
            if (_distanceToWaypoint < _waypointRange)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _currentWaypoint = _path.Waypoints[_currentTargetIndex];
                SetSpeedLimit(_currentWaypoint.TargetSpeed);
            }

            _distanceToWaypoint = Vector3.Distance(transform.position, _currentWaypoint.transform.position);
            _vectorToTarget = new Vector3(_currentWaypoint.transform.position.x, transform.position.y, _currentWaypoint.transform.position.z);
            _rotationAngleToPathNode = Vector3.SignedAngle(_vectorToTarget - transform.position, transform.forward, Vector3.up);         
        }

        _speed = _gearBox.GetSpeed();
        Steer(_rotationAngleToPathNode);
        DetectJam();
    }

    private void Steer(float angle)
    {
        if(angle > 0)
            _steerable.TurnLeft(angle);
        else if(angle < 0)
            _steerable.TurnRight(-angle);


        if (_speed < _speedLimit && !_isStucked)
        {
            Debug.Log("Ускоряюсь");
            _movable.Acceleration();
        }
        else if((_speed - _speedLimit) <= 5f && !_isStucked)
        {
            Debug.Log("Качусь по инерции");
            _movable.Deceleration();
        }
        else if ((_speed - _speedLimit) > 10f && !_isStucked)
        {
            Debug.Log("Торможу");
            _movable.Brake();
        }
    }

    private void DetectJam()
    {
        if (!_isStucked)
            _jamTime = _speed < 5 ? _jamTime += Time.deltaTime : _jamTime = 0;

        if (_jamTime > 3f)
            StartCoroutine(Reverse());
    }

    private IEnumerator Reverse()
    {
        _isStucked = true;
        _jamTime = 0f;

        float duration = Time.time + 3.0f;
        while (Time.time < duration)
        {
            Debug.Log("BAAAAAAACKKK");
            _movable.Reverse();
            yield return null;
        }

        _isStucked = false;
    }


    public void SetSpeedLimit(float speedLimit)
    {
        _speedLimit = speedLimit;
    }
}
