using UnityEngine;

public class RunToWaypointState : State
{    
    [SerializeField] private Waypoint _targetWaypoint;
    [SerializeField] private int _currentTargetIndex;
    [SerializeField] private float _angleBetweenCarAndWaypoint;
    [SerializeField, Tooltip("Ограничение скорости")] private float _speedLimit = 9999f;
    private float _speed;
    private float _distanceToWaypoint;
    private float _waypointRange = 5f;
    private Vector3 _vectorToTarget;
    private float _jamTimer;

    private bool _rivalsInFront;
    private bool _leftIsOccupied;
    private bool _righttIsOccupied;
    private float _rivalVelocity;
    private bool _rivalIsSlow;
    private Collider _rivalCollider;

    // constructor
    private Transform _transform;
    private CustomTrigger[] _customTriggers;
    [SerializeField] private AIPath _path;
    private IMovable _movable;
    private ISteerable _steerable;
    private IGearBox _gearBox;

    public RunToWaypointState(FinalStateMashine finalStateMashine,  Transform transform, CustomTrigger[] customTriggers,
        IMovable movable, ISteerable steerable, IGearBox gearBox, AIPath path) : base(finalStateMashine)
    {
        _transform = transform;
        _customTriggers = customTriggers;
        _movable = movable;
        _steerable = steerable;
        _gearBox = gearBox;
        _path = path;
    }

    public override void Enter()
    {
        if (_targetWaypoint == null)
            FindFirstWaypoint();

        EnableTriggers();
    }

    public override void Update()
    {
        if (_targetWaypoint != null)
        {
            if (_distanceToWaypoint < _waypointRange)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _targetWaypoint = _path.Waypoints[_currentTargetIndex];
                SetSpeedLimit(_targetWaypoint.TargetSpeed);
            }

            _distanceToWaypoint = Vector3.Distance(_transform.position, _targetWaypoint.transform.position);
            _vectorToTarget = new Vector3(_targetWaypoint.transform.position.x, _transform.position.y, _targetWaypoint.transform.position.z);
            _angleBetweenCarAndWaypoint = Vector3.SignedAngle(_vectorToTarget - _transform.position, _transform.forward, Vector3.up);
        }

        _speed = _gearBox.GetSpeed();

        if (_rivalsInFront)
            CalculateVelocityBetweenMeAndRival();


        if (_rivalIsSlow && _rivalsInFront)
        {
            DriveAroundRival(40);
        }
        else if (_rivalsInFront && _leftIsOccupied && _righttIsOccupied)
        {
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
        }
        else
        {
            RotateWheelsToWaypoint(_angleBetweenCarAndWaypoint);
        }

        Gas();
        DetectJam();
        RotateFrontTrigger();
        //Debug.Log("Я: " + _speed + " Противник: " + _rivalVelocity);
    }

    public override void Exit()
    {
        DisableTriggers();
    }

    private void RotateFrontTrigger()
    {
        Vector3 direction = _vectorToTarget - _customTriggers[0].transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        _customTriggers[0].transform.rotation = rotation;
    }

    private void EnableTriggers()
    {
        _customTriggers[0].EnteredTrigger += OnFrontTriggerEntered;
        _customTriggers[0].ExitedTrigger += OnFrontTriggerExited;
        _customTriggers[1].ExitedTrigger += OnLeftTriggerEntered;
        _customTriggers[1].ExitedTrigger += OnLeftTriggerExited;
        _customTriggers[2].EnteredTrigger += OnRightTriggerEntered;
        _customTriggers[2].ExitedTrigger += OnRightTriggerExited;
    }

    private void DisableTriggers()
    {
        _customTriggers[0].EnteredTrigger -= OnFrontTriggerEntered;
        _customTriggers[0].ExitedTrigger -= OnFrontTriggerExited;
        _customTriggers[1].ExitedTrigger -= OnLeftTriggerEntered;
        _customTriggers[1].ExitedTrigger -= OnLeftTriggerExited;
        _customTriggers[2].EnteredTrigger -= OnRightTriggerEntered;
        _customTriggers[2].ExitedTrigger -= OnRightTriggerExited;
    }

    private void OnFrontTriggerEntered(Collider collider)
    {
        if (collider.gameObject.layer == 6)
        {
            _rivalsInFront = true;
            _rivalCollider = collider;
        }
    }

    private void OnFrontTriggerExited(Collider collider)
    {
        if (collider.gameObject.layer == 6)
        {
            _rivalsInFront = false;
            _rivalCollider = null;
        }
    }

    private void OnLeftTriggerEntered(Collider collider) => _leftIsOccupied = true;
    private void OnLeftTriggerExited(Collider collider) => _leftIsOccupied = false;
    private void OnRightTriggerEntered(Collider collider) => _righttIsOccupied = true;
    private void OnRightTriggerExited(Collider collider) => _righttIsOccupied = false;

    private void FindFirstWaypoint()
    {
        if (_path != null && _path.Waypoints.Count > 0)
        {
            _currentTargetIndex = 0;
            _targetWaypoint = _path.Waypoints[_currentTargetIndex];
            SetSpeedLimit(_targetWaypoint.TargetSpeed);
        }
    }

    private void CalculateVelocityBetweenMeAndRival()
    {
        _rivalVelocity = Mathf.Round(_transform.InverseTransformDirection(_rivalCollider.attachedRigidbody.velocity).z * 3.6f);
        if (Mathf.Abs(_rivalVelocity - _speed) > 5f)
            _rivalIsSlow = true;
        else
            _rivalIsSlow = false;
    }

    private void DriveAroundRival(float angle)
    {
        float deltaX = _transform.position.x - _rivalCollider.transform.position.x;

        if (!_leftIsOccupied && deltaX > 0)
        {
            _steerable.TurnLeft(angle);
        }
        else if (!_righttIsOccupied && deltaX < 0)
        {
            _steerable.TurnRight(angle);
        }
    }

    private void RotateWheelsToWaypoint(float angle)
    {
        if (angle > 0 && !_leftIsOccupied)
            _steerable.TurnLeft(angle);
        else if (angle < 0 && !_righttIsOccupied)
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
        _jamTimer = _speed < 5 ? _jamTimer += Time.deltaTime : _jamTimer = 0;

        if (_jamTimer > 3f)
        {
            FinalStateMashine.SetState<ReverseState>();
            _jamTimer = 0;
        }
    }
}
