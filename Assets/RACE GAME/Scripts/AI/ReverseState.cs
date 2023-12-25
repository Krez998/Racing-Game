using UnityEngine;

public class ReverseState : State
{
    private IMovable _movable;
    private float _reverseTime;
    private float _reverseTimeTemp;

    public ReverseState(FinalStateMashine finalStateMashine, IMovable movable, float reverseTime)
        : base(finalStateMashine)
    {
        _movable = movable;
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
            _movable.Reverse();
        }
        else
            FinalStateMashine.SetState<RunToWaypointState>();
    }
}
