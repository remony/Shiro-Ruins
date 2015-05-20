﻿using UnityEngine;
using System.Collections;

public class ItemController : ItemStateHandler
{
    public Item item;
    private Animator animator;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private AudioClip pickupSound;
    private AudioSource audioSource;
    public GameObject levelManager;

    public override void onIdle()
    {

    }
    public override void onTaken()
    {

    }
	// Use this for initialization
	void Start () {
        base.Start();
        item = new Item();
        if (item.value == 0)
        {
            item.value = 100;
        }
        
        animator = gameObject.GetComponent<Animator>();
        body = gameObject.AddComponent<Rigidbody2D>();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        body.isKinematic = false;
        body.fixedAngle = true;
        body.gravityScale = 0;
        Debug.Log(item.value);
        boxCollider.size = new Vector2(0.1f, 0.1f);
        boxCollider.isTrigger = true;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        audioSource = gameObject.AddComponent<AudioSource>();
        
        int ran = Random.Range(0, 2);
        //set a random sound
        switch (ran)
        {
            case 0:
                pickupSound = Resources.Load("sounds/pickup") as AudioClip;
                break;
            case 1:
                pickupSound = Resources.Load("sounds/pickup2") as AudioClip;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}


  

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            
            audioSource.PlayOneShot(pickupSound, 1f);
            int willHeal = Random.Range(0,2);
            print(willHeal);
            if (levelManager != null)
            {
                if (willHeal == 1)
                {
                    print("healing");
                    int health = Random.Range(0, 15);
                    //coll.gameObject.GetComponent<CharacterController>().heal(health);
                    coll.GetComponent<CharacterController>().heal(health);
                }
                levelManager.GetComponent<GuiObserver>().AddScore(item.value);
            }
            
            StartCoroutine("destroy");
        }

    }


    //Allows time for sound to play before the item breaks
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.13f);
        Destroy(gameObject);

    }
}

