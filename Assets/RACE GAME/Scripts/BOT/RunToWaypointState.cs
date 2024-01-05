using UnityEngine;

public class RunToWaypointState : State
{    
    [SerializeField] private Waypoint _targetWaypoint;
    [SerializeField] private int _currentTargetIndex;
    [SerializeField] private float _angleBetweenCarAndWaypoint;
    [SerializeField] private float _speedLimit = 9999f;
    private float _speed;
    private float _distanceToWaypoint;
    private float _acceptableDistance = 8f;
    private Vector3 _vectorToTarget;
    private float _jamTimer;

    // constructor
    private Transform _transform;
    private EnvironmentDetector _rivalsDetector;
    private BOTPath _path;
    private IMovable _movable;
    private ISteerable _steerable;
    private ISpeedometer _speedometer;

    public RunToWaypointState(FinalStateMashine finalStateMashine, Transform transform, EnvironmentDetector rivalsDetector,
        IMovable movable, ISteerable steerable, ISpeedometer speedometer, BOTPath path) : base(finalStateMashine)
    {
        _transform = transform;
        _rivalsDetector = rivalsDetector;
        _movable = movable;
        _steerable = steerable;
        _speedometer = speedometer;
        _path = path;
    }

    public override void Enter()
    {
        if (_targetWaypoint == null)
            FindFirstWaypoint();
    }

    public override void Update()
    {
        if (_targetWaypoint != null)
        {
            if (_distanceToWaypoint < _acceptableDistance)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _targetWaypoint = _path.Waypoints[_currentTargetIndex];
                SetSpeedLimit(_targetWaypoint.TargetSpeed);
            }

            _distanceToWaypoint = Vector3.Distance(_transform.position, _targetWaypoint.transform.position);
            _vectorToTarget = new Vector3(_targetWaypoint.transform.position.x, _transform.position.y, _targetWaypoint.transform.position.z);
            _rivalsDetector.SetTargetWaypoint(_vectorToTarget);
            _angleBetweenCarAndWaypoint = Vector3.SignedAngle(_vectorToTarget - _transform.position, _transform.forward, Vector3.up);
        }

        if (_rivalsDetector.RivalIsSlow && _rivalsDetector.RivalsInFront)
        {
            DriveAroundRival(40);
        }
        else if (_rivalsDetector.RivalsInFront && _rivalsDetector.LeftIsOccupied && _rivalsDetector.RightIsOccupied)
        {
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
        }
        else
        {
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
        }

        _speed = _speedometer.GetSpeed();
        Gas();
        DetectJam();
    }

    private void FindFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {
            _currentTargetIndex = 0;
            _targetWaypoint = _path.Waypoints[_currentTargetIndex];
            SetSpeedLimit(_targetWaypoint.TargetSpeed);
        }
    }

    private void DriveAroundRival(float angle)
    {
        float deltaX = _transform.position.x - _rivalsDetector.Rival.position.x;

        if (!_rivalsDetector.LeftIsOccupied && deltaX > 0)
        {
            _steerable.TurnLeft(angle);
        }
        else if (!_rivalsDetector.RightIsOccupied && deltaX < 0)
        {
            _steerable.TurnRight(angle);
        }
    }

    private void RotateWheelsToWaypoint(float angle)
    {
        if (angle > 0 && !_rivalsDetector.LeftIsOccupied)
            _steerable.TurnLeft(angle);
        else if (angle < 0 && !_rivalsDetector.RightIsOccupied)
            _steerable.TurnRight(-angle);
    }

    private void Gas()
    {
        if (_speed < _speedLimit)
        {
            _movable.Acceleration();
        }
        else if ((_speed - _speedLimit) <= 5f)
        {
            _movable.Deceleration();
        }
        else if ((_speed - _speedLimit) > 10f)
        {
            _movable.Brake();
        }
    }

    public void SetSpeedLimit(float speedLimit)
    {
        _speedLimit = speedLimit;
    }

    private void DetectJam()
    {
        _jamTimer = _speed < 1 ? _jamTimer += Time.deltaTime : _jamTimer = 0;

        if (_jamTimer > 1.5f)
            RotateWheelsToWaypoint(-_angleBetweenCarAndWaypoint);
        if (_jamTimer > 2.2f)
            MoveBackward();
    }

    private void MoveBackward()
    {
        FinalStateMashine.SetState<ReverseState>();
        _jamTimer = 0;
    }
}
