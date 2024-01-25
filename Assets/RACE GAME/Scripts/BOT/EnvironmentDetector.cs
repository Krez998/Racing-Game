using UnityEngine;

public enum WaypointPosition
{
    Left,
    Right,
    Middle
}

[RequireComponent(typeof(GearBox))]
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
    private float _frontTriggerAngle;
    private GearBox _gearBox;


    [SerializeField] private bool _rivalsInFront;
    [SerializeField] private bool _leftIsOccupied;
    [SerializeField] private bool _rightIsOccupied;
    private float _rivalVelocity;
    [SerializeField] private bool _rivalIsSlow;
    private Collider _rivalCollider;
    private Vector3 _targetPosition;

    public void SetTargetWaypoint(Vector3 target)
    {
        _targetPosition = target;
    }

    private void Awake()
    {
        _gearBox = GetComponent<GearBox>();
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
        _angleBetweenCarAndWaypoint = Vector3.SignedAngle(transform.forward, _targetPosition - transform.position, Vector3.up);

        if (_angleBetweenCarAndWaypoint < 0)
            _waypointPosition = WaypointPosition.Left;
        else if (_angleBetweenCarAndWaypoint > 0)
            _waypointPosition = WaypointPosition.Right;
        else
            _waypointPosition = WaypointPosition.Middle;
    }

    private void RotateFrontTrigger()
    {
        _frontTriggerAngle = _angleBetweenCarAndWaypoint;
        if (_frontTriggerAngle > 40f)
            _frontTriggerAngle = 40f;
        else if (_frontTriggerAngle < -40f)
            _frontTriggerAngle = -40f;

        _triggers[0].transform.localRotation = Quaternion.Euler(0f, _frontTriggerAngle, 0f);
        //_triggers[0].transform.localEulerAngles = new Vector3(0f, frontTriggerAngle, 0f);

        //Vector3 direction = _targetPosition - _triggers[0].transform.position;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        //_triggers[0].transform.rotation = rotation;
    }

    private void CalculateVelocityBetweenMeAndRival()
    {
        _rivalVelocity = Mathf.Round(transform.InverseTransformDirection(_rivalCollider.attachedRigidbody.velocity).z * 3.6f);
        if (_rivalVelocity < _gearBox.Speed && _gearBox.Speed - _rivalVelocity > 5f)
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