using UnityEngine;

[RequireComponent(typeof(ISpeedometer))]
public class RivalsDetector : MonoBehaviour
{
    public Collider RivalCollier => _rivalCollider;
    public bool RivalIsSlow => _rivalIsSlow;
    public bool LeftIsOccupied => _leftIsOccupied;
    public bool RightIsOccupied => _rightIsOccupied;
    public bool RivalsInFront => _rivalsInFront;
    public Transform Rival => _rivalCollider.transform;

    [SerializeField] private LayerMask _carLayer;
    [SerializeField] private CustomTrigger[] _triggers;

    private ISpeedometer _speedometer;

    private bool _rivalsInFront;
    private bool _leftIsOccupied;
    private bool _rightIsOccupied;
    private float _rivalVelocity;
    private bool _rivalIsSlow;
    private Collider _rivalCollider;
    private Vector3 _vectorToTarget;

    public void SetTargetWaypoint(Vector3 target)
    {
        _vectorToTarget = target;
    }

    private void Awake()
    {
        _speedometer = GetComponent<Speedometer>();
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

        if (_rivalsInFront)
            CalculateVelocityBetweenMeAndRival();

        //Debug.Log("Я: " + _gearBox.GetSpeed() + " Противник: " + _rivalVelocity);
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

    private void OnLeftTriggerEntered(Collider collider) => _leftIsOccupied = true;
    private void OnLeftTriggerExited(Collider collider) => _leftIsOccupied = false;
    private void OnRightTriggerEntered(Collider collider) => _rightIsOccupied = true;
    private void OnRightTriggerExited(Collider collider) => _rightIsOccupied = false;
}