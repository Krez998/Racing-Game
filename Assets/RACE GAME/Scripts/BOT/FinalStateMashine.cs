using System;
using System.Collections.Generic;

public class FinalStateMashine
{
    private State _currentState;
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();
    private Type _type;

    public void AddState(State state) => _states.Add(state.GetType(), state);

    public void SetState<T>() where T : State
    {
        _type = typeof(T);

        if (_currentState != null && _currentState.GetType() == _type)
            return;

        if (_states.TryGetValue(_type, out var targetState))
        {
            _currentState?.Exit();
            _currentState = targetState;
            _currentState.Enter();
        }
    }

    public void Update() => _currentState?.Update();
}