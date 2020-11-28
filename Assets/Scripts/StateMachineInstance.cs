
using UnityEditor;
using UnityEngine;

public class StateMachineInstance<T> : StateMachineInstance where T : StateMachineInstance
{
    public StateMachine<T> sm;

    public void InitStateMachine()
    {
        sm = new StateMachine<T>
        {
            smi = this
        };

        InitializeStates();
        sm.SetState(sm.defaultState);
        sm.Start();
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, sm?.currentState == null ? "null" : sm.currentState.name);
    }

    public StateMachine<T>.State AddState(string name)
    {
        StateMachine<T>.State state = new StateMachine<T>.State(name, this);
        sm.RegisterState(state);
        return state;
    }

    private void Update()
    {
        sm.Tick();
    }
}

public class StateMachineInstance : MonoBehaviour
{
    public virtual void InitializeStates()
    {
    }
}

