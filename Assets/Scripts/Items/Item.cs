using UnityEngine;
using System.Collections;

public class Item
{
    public int value
    {
        get;
        set;
    }

    public Rigidbody2D body
    {
        get;
        set;
    }

    public BoxCollider2D boxCollider
    {
        get;
        set;
    }

    public AudioClip pickupSound
    {
        get;
        set;
    }

    public AudioSource audioSource
    {
        get;
        set;
    }

    public GameObject levelManager
    {
        get;
        set;
    }
}
