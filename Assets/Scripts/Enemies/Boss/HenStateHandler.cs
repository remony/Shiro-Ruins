using UnityEngine;
using System.Collections;
using System;

public abstract class HenStateHandler : MonoBehaviour
{
    public abstract void onIdle();
    public abstract void onDeath();
    public abstract void onWalking();
    public abstract void onAttacking();

    public enum State
    {
        STATE_IDLE,
        STATE_DEATH,
        STATE_WALKING,
        STATE_ATTACKING
    };

    public State state;

    protected void Start()
    {
        state = State.STATE_IDLE;
    }

    protected void Update()
    {
        switch (state)
        {
            case State.STATE_IDLE:
                onIdle();
                break;
            case State.STATE_WALKING:
                onWalking();
                break;
            case State.STATE_DEATH:
                onDeath();
                break;
            case State.STATE_ATTACKING:
                onAttacking();
                break;
        }
    }

}
