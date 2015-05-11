using UnityEngine;
using System.Collections;

public abstract class GUIStateHandle : MonoBehaviour
{

    public abstract void onProgress();
    public abstract void onPause();
    public abstract void onUnPause();
    public abstract void onComplete();
    public abstract void onDeath();
    public abstract void onStart();

    public enum State
    {
        STATE_INPROGRESS,
        STATE_PAUSED,
        STATE_COMPLETE,
        STATE_UNPAUSED,
        STATE_DEATH,
        STATE_START
    };

    public State state;

    // Use this for initialization
    protected void Start()
    {
        state = State.STATE_START; //Is not required
    }

    // Update is called once per frame
    protected void Update()
    {
        switch (state)
        {
            case State.STATE_INPROGRESS:
                onProgress();
                break;
            case State.STATE_PAUSED:
                onPause();
                break;
            case State.STATE_UNPAUSED:
                onUnPause();
                break;
            case State.STATE_COMPLETE:
                onComplete();
                break;
            case State.STATE_DEATH:
                onDeath();
                break;
            case State.STATE_START:
                onStart();
                break;
        }
    }
}
