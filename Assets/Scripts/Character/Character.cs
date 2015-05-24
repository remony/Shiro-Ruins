using UnityEngine;
using System.Collections;
[System.Serializable]
public class Character {
    [UnityEngine.SerializeField]
    public int health
    {
        get;
        set;
    }
    [UnityEngine.SerializeField]
    public float speed
    {
        get;
        set;
    }
    [UnityEngine.SerializeField]
    public float jump
    {
        get;
        set;
    }

    public float cooldownLimit
    {
        get;
        set;
    }
    public float cooldownTimer
    {
        get;
        set;
    }
    public float cooldownStart
    {
        get;
        set;
    }

    public float blastCooldownLimit
    {
        get;
        set;
    }

    public float blastCooldownTimer
    {
        get;
        set;
    }

    public float blastCooldownStart
    {
        get;
        set;
    }

    public Vector2 startingPosition
    {
        get;
        set;
    }
}
