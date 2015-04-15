using UnityEngine;
using System.Collections;

public abstract class SignStateHandler : MonoBehaviour
{

    public abstract void onIdle();

    public abstract void onPlayer();



    public enum State
    {
        STATE_IDLE,
        STATE_PLAYER
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
            case State.STATE_PLAYER:
                onPlayer();
                break;
        }
    }

}