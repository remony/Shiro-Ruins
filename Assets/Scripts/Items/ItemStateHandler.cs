using UnityEngine;
using System.Collections;
using System;

public abstract class ItemStateHandler : MonoBehaviour
{

    public abstract void onIdle();

    public abstract void onTaken();


    public enum State
    {
        STATE_IDLE,
        STATE_TAKEN
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
            case State.STATE_TAKEN:
                onTaken();
                break;
           
        }
    }
}
