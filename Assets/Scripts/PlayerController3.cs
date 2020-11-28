using System;
using UnityEngine;
using UnityEngine.UI;
using static StateMachine<PlayerController3>;

public class PlayerController3 : StateMachineInstance<PlayerController3>
{
    [HideInInspector] public bool isGrounded;

    private void Awake()
    {
        InitStateMachine();
    }

    public override void InitializeStates()
    {
        State walking = AddState("walking");
        State jumping = AddState("jumping");

        sm.defaultState = walking;

        walking
            .Enter(smi => smi.Test())
            .Transition(jumping, SomeCondition)
            .Exit(smi => Debug.Log("<walking leave>"));
        jumping
            .Transition(walking, () => !SomeCondition())
            .Enter(smi => Debug.Log("<jumping enter>"));

    }

    private bool SomeCondition()
    {
        return Input.GetButton("Jump");
    }

    private void Test()
    {
        Debug.Log("<walking enter>");
    }
}
