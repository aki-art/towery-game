using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<SMIType> where SMIType : StateMachineInstance
{
    public StateMachineInstance smi;
    public State defaultState;
    public State currentState;
    Dictionary<string, State> states = new Dictionary<string, State>();
    List<Transition> currentTransitions = new List<Transition>();
    List<Transition> anyTransitions = new List<Transition>();
    static List<Transition> EmptyTransitions = new List<Transition>(0);
    private bool running = false;

    public void Start() => running = true;
    public void Stop() => running = false;

    public void Tick()
    {
        if (!running) return;

        var transition = GetNextTransition();
        if (transition != null)
            SetState(transition.To);

        ExecuteActions(currentState?.tickActions);
    }

    private Transition GetNextTransition()
    {
        foreach (var item in currentState.transitions)
        {
            if (item.Condition())
                return item;
        }

        return null;
    }

    public void RegisterState(State state)
    {
        states[state.name] = state;
    }

    public void ScheduleState(State state, float time)
    {
        Game.Instance.StartCoroutine(SetStateDelayed(state, time));
    }

    IEnumerator SetStateDelayed(State state, float time)
    {
        yield return new WaitForSeconds(time);
        SetState(state);
    }

    public void SetState(State state)
    {
        Debug.Log("setting state: " + state.name);
        if (state == currentState)
            return;

        if(currentState != null)
        {
            Debug.Log("executing exit: " + currentState.name);
            ExecuteActions(currentState.exitActions);
        }
        
        currentState = state;
        
        if (currentState.transitions == null)
            currentState.transitions = EmptyTransitions;

        Debug.Log("executing enter: " + currentState?.name);
        ExecuteActions(currentState?.enterActions);
    }

    protected State GetState(string name)
    {
        if (states.TryGetValue(name, out State result))
            return result;
        return null;
    }

    private void ExecuteActions(List<object> actions)
    {
        if (actions == null || !running) return;
        foreach (object action in actions)
        {
            State.Callback a = (State.Callback)action;
            a((SMIType)smi);
        }
    }

    public void AddTransition(State from, State to, Func<bool> predicate)
    {
        if (from.transitions == null)
            from.transitions = new List<Transition>();

        from.transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(State state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    public class Transition
    {
        public Func<bool> Condition { get; }
        public State To { get; }

        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
    
    public class State
    {
        public string name;
        public StateMachineInstance<SMIType> smi;

        public delegate void Callback(SMIType smi);

        public List<Transition> transitions;

        public List<object> enterActions;
        public List<object> exitActions;
        public List<object> tickActions;

        public State(string name, StateMachineInstance<SMIType> smi)
        {
            this.name = name;
            this.smi = smi;
        }

        public State Update(Callback callback)
        {
            if (tickActions == null)
                tickActions = new List<object>();

            tickActions.Add(callback);
            return this;
        }

        public State Enter(Callback callback)
        {
            if (enterActions == null)
                enterActions = new List<object>();

            enterActions.Add(callback);
            return this;
        }

        public State Exit(Callback callback)
        {
            if (exitActions == null)
                exitActions = new List<object>();

            exitActions.Add(callback);
            return this;
        }

        public State Transition(State state, Func<bool> predicate)
        {
            smi.sm.AddTransition(this, state, predicate);
            return this;
        }

        public State GoTo(State state)
        {
            smi.sm.SetState(state);
            return this;
        }

        public State GoToInSeconds(State state, float seconds)
        {
            smi.sm.ScheduleState(state, seconds);
            return this;
        }
    }
}
