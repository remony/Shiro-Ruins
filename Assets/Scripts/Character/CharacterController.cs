using UnityEngine;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler    {


    private bool isFacingRight = false;
    private float velocityHorizontal = 0f;
    private float velocityVertical = 0f;
    public GameObject spritesheet;
    private Animator animator;
    private GameObject player;
    private Rigidbody2D body;
    private Character character;

    public override void onIdle()
    {
        body.gravityScale = 150;
        body.mass = 10;
        if (velocityHorizontal > 0 || velocityHorizontal < 0)
        {
            state = State.STATE_WALKING;
        }
    }

    public override void onWalking()
    {
        body.gravityScale = 150;
        body.mass = 10;
        if(velocityHorizontal == 0)
        {
            state = State.STATE_IDLE;
        }
        body.velocity = new Vector2(velocityHorizontal * character.speed, body.velocity.y);
    }

    public override void onJumping()
    {
        body.gravityScale = 150;
        body.mass = 10;
    }

    public override void onSwimming()
    {
        body.gravityScale = 80;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * (character.speed / 4f), velocityVertical * (character.speed / 4f));
    }

    public override void onClimbing()
    {
        body.gravityScale = 0;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * character.speed, velocityVertical * character.speed);
        
    }

    void Start()
    {
        base.Start();
        character = new Character();
        character.health = 100;
        character.jump = 450;
        character.speed = 210;
        print(character.health);

        player = this.gameObject;
        animator = spritesheet.GetComponent<Animator>();
        body = player.AddComponent<Rigidbody2D>();

        body.isKinematic = false;
        body.fixedAngle = true;
    }
    

    void Update()
    {
        base.Update();

        movement();
        
        //print(state);
    }

    void movement()
    {
        velocityHorizontal = Input.GetAxisRaw("Horizontal");
        velocityVertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("speed", Mathf.Abs(velocityHorizontal));
        
        


        if (velocityHorizontal > 0 && !isFacingRight)
            flip();
        else if (velocityHorizontal < 0 && isFacingRight)
            flip();

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            jump();
        }
    }

    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, character.jump);
    }

    private void flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = body.transform.localScale;
        theScale.x *= -1;
        body.transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Untagged"))
        {
            state = State.STATE_CLIMBING;
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            state = State.STATE_SWIMMING;
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Untagged"))
        {
            state = State.STATE_IDLE;
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            state = State.STATE_IDLE;
        }
    }



}
