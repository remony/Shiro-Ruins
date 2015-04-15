using UnityEngine;
using System.Collections;
using System;

public abstract class CharacterStateHandler : MonoBehaviour
{

    public abstract void onIdle();

    public abstract void onWalking();

    public abstract void onJumping();

    public abstract void onSwimming();

    public abstract void onClimbing();

    public abstract void onMovingPlatform();

    public abstract void onFalling();

    public abstract void onUnderWater();

    public abstract void onDeath();

    public  enum State
    {
        STATE_IDLE,
        STATE_WALKING,
        STATE_JUMPING,
        STATE_SWIMMING,
        STATE_CLIMBING,
        STATE_MOVINGPLATFORM,
        STATE_FALLING,
        STATE_UNDERWATER,
        STATE_DEATH
    };

    public State state;

    // Use this for initialization
    protected void Start()
    {
        
        state = State.STATE_IDLE; //Is not required
    }

    // Update is called once per frame
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
            case State.STATE_JUMPING:
                onJumping();
                break;
            case State.STATE_SWIMMING:
                onSwimming();
                break;
            case State.STATE_CLIMBING:
                onClimbing();
                break;
            case State.STATE_MOVINGPLATFORM:
                onMovingPlatform();
                break;
            case State.STATE_FALLING:
                onFalling();
                break;
            case State.STATE_UNDERWATER:
                onUnderWater();
                break;
            case State.STATE_DEATH:
                onDeath();
                break;
        }
    }

}
