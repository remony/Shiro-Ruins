using UnityEngine;
using System.Collections;

public class BreakablePlatform
{
    //The time to which it will wait until it breaks
    public float timeToLive
    {
        get;
        set;
    }

    //The time to which it will wait after it has been broken to recover itself
    public float timeToRecover
    {
        get;
        set;
    }

    //The position to which is has been spawned [Experimental: may not be used]
    public Transform spawn
    {
        get;
        set;
    }
}
