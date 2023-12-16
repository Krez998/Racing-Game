using System;
using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Path Params")]
    [SerializeField] private AIPath _path;
    [SerializeField] private Waypoint _targetWaypoint;
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

    public float _jamTimer;
    public bool _isStucked;
    public bool _isOverturned;
    public float _rotationZ;

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
            _targetWaypoint = _path.Waypoints[_currentTargetIndex];
            SetSpeedLimit(_targetWaypoint.TargetSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (_targetWaypoint != null)
        {
            if (_distanceToWaypoint < _waypointRange)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _path.Waypoints.Count;
                _targetWaypoint = _path.Waypoints[_currentTargetIndex];
                SetSpeedLimit(_targetWaypoint.TargetSpeed);
            }

            _distanceToWaypoint = Vector3.Distance(transform.position, _targetWaypoint.transform.position);
            _vectorToTarget = new Vector3(_targetWaypoint.transform.position.x, transform.position.y, _targetWaypoint.transform.position.z);
            _rotationAngleToPathNode = Vector3.SignedAngle(_vectorToTarget - transform.position, transform.forward, Vector3.up);         
        }

        _speed = _gearBox.GetSpeed();
        Steer(_rotationAngleToPathNode);
        DetectJam();
        CheckOverturn();
    }

    private void Steer(float angle)
    {
        if(angle > 0)
            _steerable.TurnLeft(angle);
        else if(angle < 0)
            _steerable.TurnRight(-angle);


        if (_speed < _speedLimit && !_isStucked)
        {
            //Debug.Log("Ускоряюсь");
            _movable.Acceleration();
        }
        else if((_speed - _speedLimit) <= 5f && !_isStucked)
        {
            //Debug.Log("Качусь по инерции");
            _movable.Deceleration();
        }
        else if ((_speed - _speedLimit) > 10f && !_isStucked)
        {
            //Debug.Log("Торможу");
            _movable.Brake();
        }
    }

    private void DetectJam()
    {
        if (!_isStucked)
            _jamTimer = _speed < 5 ? _jamTimer += Time.deltaTime : _jamTimer = 0;

        if (_jamTimer > 3f && !_isOverturned)
            StartCoroutine(Reverse());
    }

    private void CheckOverturn()
    {
        _rotationZ = transform.rotation.eulerAngles.z;

        if (transform.rotation.eulerAngles.z <= 180f)
            _rotationZ = transform.rotation.eulerAngles.z;
        else
            _rotationZ = transform.rotation.eulerAngles.z - 360f;

        if (Mathf.Abs(_rotationZ) > 80f)
        {
            _isOverturned = true;

            if (_jamTimer > 5f)
            {
                _jamTimer = 0f; // исправляет баг с лишним задним ходом, когда машина встает на колеса
                transform.LookAt(_targetWaypoint.transform);
                transform.position = _targetWaypoint.transform.position;
            }
        }
        else
            _isOverturned = false;
    }

    private IEnumerator Reverse()
    {
        _isStucked = true;
        _jamTimer = 0f;

        float duration = Time.time + 3.0f;
        while (Time.time < duration)
        {
            //Debug.Log("BAAAAAAACKKK");
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
