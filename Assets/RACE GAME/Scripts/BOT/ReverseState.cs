using UnityEngine;

public class ReverseState : State
{
    private EnvironmentDetector _environmentDetector;
    private IMovable _movable;
    private ISteerable _steerable;
    private float _reverseTime;
    private float _reverseTimeTemp;

    public ReverseState(FinalStateMashine finalStateMashine, 
        EnvironmentDetector environmentDetector, IMovable movable, ISteerable steerable, float reverseTime)
        : base(finalStateMashine)
    {
        _environmentDetector = environmentDetector;
        _movable = movable;
        _steerable = steerable;
        _reverseTime = reverseTime;
    }

    public override void Enter()
    {
        _reverseTimeTemp = _reverseTime;
    }

    public override void Update()
    {
        _reverseTimeTemp -= Time.deltaTime;

        if (_reverseTimeTemp > 0)
        {
            if(_environmentDetector.WaypointPosition == WaypointPosition.Left)
                _steerable.TurnRight(40);
            else if(_environmentDetector.WaypointPosition == WaypointPosition.Right)
                _steerable.TurnLeft(40);

            _movable.Reverse();
        }
        else
            FinalStateMashine.SetState<RunToWaypointState>();
    }
}
