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

    public Rigidbody2D body
    {
        get;
        set;
    }

    public bool musicPlaying
    {
        get;
        set;
    }

    public float velocityHorizontal
    {
        get;
        set;
    }
    public float velocityVertical
    {
        get;
        set;
    }

    public Animator animator
    {
        get;
        set;
    }
    public GameObject player
    {
        get;
        set;
    }

    public Transform previousPostiton
    {
        get;
        set;
    }

    public bool moveJump
    {
        get;
        set;
    }
}
