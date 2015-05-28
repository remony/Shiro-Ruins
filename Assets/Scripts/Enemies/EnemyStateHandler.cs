using UnityEngine;
using System.Collections;
using System;

public abstract class EnemyStateHandler : MonoBehaviour
{
    public abstract void onIdle();

    public abstract void onWalking();

    public abstract void onFollow();

    public abstract void onAttacking();

    public enum State
    {
        STATE_IDLE,
        STATE_WALKING,
        STATE_FOLLOWING,
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
            case State.STATE_FOLLOWING:
                onFollow();
                break;
            case State.STATE_ATTACKING:
                onAttacking();
                break;
        }
    }

}