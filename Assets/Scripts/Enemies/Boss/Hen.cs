using UnityEngine;
using System.Collections;

public class Hen
{
    public int health
    {
        get;
        set;
    }

    public float size
    {
        get;
        set;
    }

    public int attackPower
    {
        get;
        set;
    }

    public Animator animator
    {
        get;
        set;
    }

    public int wanderingDistance
    {
        get;
        set;
    }

    public Vector2 startingPosition
    {
        get;
        set;
    }

    public bool facingRight
    {
        get;
        set;
    }

    public Rigidbody2D body
    {
        get;
        set;
    }

    public GameObject target
    {
        get;
        set;
    }

    public float speed
    {
        get;
        set;
    }
}