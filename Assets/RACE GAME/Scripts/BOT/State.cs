public abstract class State
{
    protected readonly FinalStateMashine FinalStateMashine;

    protected State(FinalStateMashine finalStateMashine)
    {
        FinalStateMashine = finalStateMashine;
    }

    //protected Transform Transform;
    //protected IMovable Movable;
    //protected ISteerable Steerable;
    //protected IGearBox GearBox;

    //protected State(Transform transform, IMovable movable, ISteerable steerable, IGearBox gearBox)
    //{
    //    Transform = transform;
    //    Movable = movable;
    //    Steerable = steerable;
    //    GearBox = gearBox;
    //}

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
