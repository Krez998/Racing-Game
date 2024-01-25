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
    private Vector3 _targetPosition;
    private float _jamTimer;

    private Transform _transform;
    private EnvironmentDetector _environmentDetector;
    private BOTPath _path;
    private IMovable _movable;
    private ISteerable _steerable;
    private GearBox _gearBox;

    public RunToWaypointState(FinalStateMashine finalStateMashine, Transform transform, 
        EnvironmentDetector environmentDetector, IMovable movable, ISteerable steerable, 
        GearBox gearBox, BOTPath path) : base(finalStateMashine)
    {
        _transform = transform;
        _environmentDetector = environmentDetector;
        _movable = movable;
        _steerable = steerable;
        _gearBox = gearBox;
        _path = path;
    }

    public override void Enter()
    {
        if (_targetWaypoint == null)
            AssignFirstWaypoint();
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
            _targetPosition = new Vector3(_targetWaypoint.transform.position.x, _transform.position.y, _targetWaypoint.transform.position.z);
            _environmentDetector.SetTargetWaypoint(_targetPosition);
            _angleBetweenCarAndWaypoint = Vector3.SignedAngle(_transform.forward, _targetPosition - _transform.position, Vector3.up);
        }

        if (_environmentDetector.RivalIsSlow && _environmentDetector.RivalsInFront)
            DriveAroundRival(40);
        else if (_environmentDetector.RivalsInFront && _environmentDetector.LeftIsOccupied && _environmentDetector.RightIsOccupied)
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
        else
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);

        _speed = _gearBox.Speed;
        Move();
        DetectJam();
    }

    private void DriveAroundRival(float angle)
    {
        float rivalPosX = _transform.InverseTransformPoint(_environmentDetector.RivalTransform.position).x;

        // åñëè öåëü ÑËÅÂÀ, à ïðåïÿòñòâèå ÑËÅÂÀ, òîãäà îáãîí ÑÏÐÀÂÀ
        if (_angleBetweenCarAndWaypoint < 0 && rivalPosX < 0 && !_environmentDetector.RightIsOccupied)
        {
            _steerable.TurnRight(angle);
        }
        // åñëè öåëü ÑËÅÂÀ, à ïðåïÿòñòâèå ÑÏÐÀÂÀ, òîãäà îáãîí ÑËÅÂÀ
        else if (_angleBetweenCarAndWaypoint < 0 && rivalPosX > 0 && !_environmentDetector.LeftIsOccupied)
        {
            _steerable.TurnLeft(angle);
        }
        // åñëè öåëü ÑÏÐÀÂÀ, à ïðåïÿòñòâèå ÑËÅÂÀ, òîãäà îáãîí ÑÏÐÀÂÀ
        else if (_angleBetweenCarAndWaypoint > 0 && rivalPosX < 0 && !_environmentDetector.RightIsOccupied)
        {
            _steerable.TurnRight(angle);
        }
        // åñëè öåëü ÑÏÐÀÂÀ, à ïðåïÿòñòâèå ÑÏÐÀÂÀ, òîãäà îáãîí ÑËÅÂÀ
        else if (_angleBetweenCarAndWaypoint > 0 && rivalPosX > 0 && !_environmentDetector.LeftIsOccupied)
        {
            _steerable.TurnLeft(angle);
        }
        else
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
    }

    private void AssignFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {          
            _currentTargetIndex = 0;
            _targetWaypoint = _path.Waypoints[_currentTargetIndex];
            _distanceToWaypoint = Vector3.Distance(_transform.position, _targetWaypoint.transform.position);
            SetSpeedLimit(_targetWaypoint.TargetSpeed);
        }
    }

    private void RotateWheelsToWaypoint(float angle)
    {
        if (angle < 0) // && !_environmentDetector.LeftIsOccupied
            _steerable.TurnLeft(angle);
        else if (angle > 0) //  && !_environmentDetector.RightIsOccupied
            _steerable.TurnRight(angle);
    }

    private void Move()
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

    public void SetSpeedLimit(float speedLimit) => _speedLimit = speedLimit;

    private void DetectJam()
    {
        _jamTimer = _speed < 1 ? _jamTimer += Time.deltaTime : _jamTimer = 0;

        if (_jamTimer > 2f)
            MoveBackward();
    }

    private void MoveBackward()
    {
        FinalStateMashine.SetState<ReverseState>();
        _jamTimer = 0;
    }
}
