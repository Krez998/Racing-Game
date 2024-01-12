public abstract class State
{
    protected readonly FinalStateMashine FinalStateMashine;

    protected State(FinalStateMashine finalStateMashine)
    {
        FinalStateMashine = finalStateMashine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
