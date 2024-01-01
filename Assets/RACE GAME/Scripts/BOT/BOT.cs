using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(EnvironmentDetector))]
[RequireComponent(typeof(IMovable))]
[RequireComponent(typeof(ISteerable))]
[RequireComponent(typeof(ISpeedometer))]
public class BOT : MonoBehaviour
{
    [SerializeField] private BOTPath _path;
    [SerializeField] private float _reverseTime;

    private FinalStateMashine _finalStateMashine;
    private EnvironmentDetector _rivalsDetector;
    private IMovable _movable;
    private ISteerable _steerable;
    private ISpeedometer _speedometer;

    //[SerializeField] private bool _isOverturned;
    //private float _rotationZ;
    //private float _overturnTimer;

    private void Awake()
    {
        _rivalsDetector = GetComponent<EnvironmentDetector>();
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
        _speedometer = GetComponent<ISpeedometer>();

        _finalStateMashine = new FinalStateMashine();
        _finalStateMashine.AddState(new RunToWaypointState(_finalStateMashine, transform, _rivalsDetector, _movable, _steerable, _speedometer, _path));
        _finalStateMashine.AddState(new ReverseState(_finalStateMashine, _movable, _reverseTime));

        _finalStateMashine.SetState<RunToWaypointState>();
    }

    private void FixedUpdate()
    {
        _finalStateMashine.Update();
        //CheckOverturn();
    }


    //private void DetectJam()
    //{
    //    if (!_isStucked)
    //        _jamTimer = _speed < 5 ? _jamTimer += Time.deltaTime : _jamTimer = 0;

    //    if (_jamTimer > 3f && !_isOverturned)
    //        StartCoroutine(Reverse());
    //}


    //private void CheckOverturn()
    //{
    //    _rotationZ = transform.rotation.eulerAngles.z;

    //    if (transform.rotation.eulerAngles.z <= 180f)
    //        _rotationZ = transform.rotation.eulerAngles.z;
    //    else
    //        _rotationZ = transform.rotation.eulerAngles.z - 360f;

    //    if (Mathf.Abs(_rotationZ) > 80f)
    //    {
    //        _overturnTimer += Time.deltaTime;

    //        if (_overturnTimer > 5f)
    //        {
    //            _overturnTimer = 0f;
    //            transform.LookAt(_targetWaypoint.transform);
    //            transform.position = _targetWaypoint.transform.position;
    //        }
    //    }
    //}
}
