using UnityEngine;

public enum WaypointPosition
{
    Left,
    Right,
    Middle
}

[RequireComponent(typeof(ISpeedometer))]
public class EnvironmentDetector : MonoBehaviour
{
    public bool RivalIsSlow => _rivalIsSlow;
    public bool LeftIsOccupied => _leftIsOccupied;
    public bool RightIsOccupied => _rightIsOccupied;
    public bool RivalsInFront => _rivalsInFront;
    public Transform RivalTransform => _rivalCollider.transform;
    public WaypointPosition WaypointPosition => _waypointPosition;

    [SerializeField] private LayerMask _carLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private CustomTrigger[] _triggers;
    [SerializeField] private WaypointPosition _waypointPosition;

    private float _angleBetweenCarAndWaypoint;
    private ISpeedometer _speedometer;

    public bool _rivalsInFront;
    public bool _leftIsOccupied;
    public bool _rightIsOccupied;
    private float _rivalVelocity;
    public bool _rivalIsSlow;
    private Collider _rivalCollider;
    private Vector3 _vectorToTarget;

    public void SetTargetWaypoint(Vector3 target)
    {
        _vectorToTarget = target;
    }

    private void Awake()
    {
        _speedometer = GetComponent<ISpeedometer>();
    }

    private void OnEnable()
    {
        EnableTriggers();
    }

    private void OnDisable()
    {
        DisableTriggers();
    }

    private void Update()
    {
        RotateFrontTrigger();
        CheckWaypointPosition();

        if (_rivalsInFront)
            CalculateVelocityBetweenMeAndRival();

        //Debug.Log("Я: " + _gearBox.GetSpeed() + " Противник: " + _rivalVelocity);
    }

    private void CheckWaypointPosition()
    {
        _angleBetweenCarAndWaypoint = Vector3.SignedAngle(transform.forward, _vectorToTarget - transform.position, Vector3.up);

        if (_angleBetweenCarAndWaypoint < 0)
            _waypointPosition = WaypointPosition.Left;
        else if (_angleBetweenCarAndWaypoint > 0)
            _waypointPosition = WaypointPosition.Right;
        else
            _waypointPosition = WaypointPosition.Middle;
    }

    private void RotateFrontTrigger()
    {
        Vector3 direction = _vectorToTarget - _triggers[0].transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        _triggers[0].transform.rotation = rotation;
    }

    private void CalculateVelocityBetweenMeAndRival()
    {
        _rivalVelocity = Mathf.Round(transform.InverseTransformDirection(_rivalCollider.attachedRigidbody.velocity).z * 3.6f);
        if (Mathf.Abs(_rivalVelocity - _speedometer.GetSpeed()) > 5f)
            _rivalIsSlow = true;
        else
            _rivalIsSlow = false;
    }

    private void EnableTriggers()
    {
        _triggers[0].EnteredTrigger += OnFrontTriggerEntered;
        _triggers[0].ExitedTrigger += OnFrontTriggerExited;
        _triggers[1].EnteredTrigger += OnLeftTriggerEntered;
        _triggers[1].ExitedTrigger += OnLeftTriggerExited;
        _triggers[2].EnteredTrigger += OnRightTriggerEntered;
        _triggers[2].ExitedTrigger += OnRightTriggerExited;
    }

    private void DisableTriggers()
    {
        _triggers[0].EnteredTrigger -= OnFrontTriggerEntered;
        _triggers[0].ExitedTrigger -= OnFrontTriggerExited;
        _triggers[1].EnteredTrigger -= OnLeftTriggerEntered;
        _triggers[1].ExitedTrigger -= OnLeftTriggerExited;
        _triggers[2].EnteredTrigger -= OnRightTriggerEntered;
        _triggers[2].ExitedTrigger -= OnRightTriggerExited;
    }

    private void OnFrontTriggerEntered(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & _carLayer) != 0)
        {
            _rivalsInFront = true;
            _rivalCollider = collider;
        }
    }

    private void OnFrontTriggerExited(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & _carLayer) != 0)
        {
            _rivalsInFront = false;
            _rivalCollider = null;
        }
    }

    private void OnLeftTriggerEntered(Collider collider)
    {
        if ((((1 << collider.gameObject.layer) & _carLayer) != 0) || (((1 << collider.gameObject.layer) & _wallLayer) != 0))
        {
            _leftIsOccupied = true;
        }
    }

    private void OnLeftTriggerExited(Collider collider)
    {
        if ((((1 << collider.gameObject.layer) & _carLayer) != 0) || (((1 << collider.gameObject.layer) & _wallLayer) != 0))
        {
            _leftIsOccupied = false;
        }
    }

    private void OnRightTriggerEntered(Collider collider)
    {
        if ((((1 << collider.gameObject.layer) & _carLayer) != 0) || (((1 << collider.gameObject.layer) & _wallLayer) != 0))
        {
            _rightIsOccupied = true;
        }
    }

    private void OnRightTriggerExited(Collider collider)
    {
        if ((((1 << collider.gameObject.layer) & _carLayer) != 0) || (((1 << collider.gameObject.layer) & _wallLayer) != 0))
        {
            _rightIsOccupied = false;
        }
    }
}