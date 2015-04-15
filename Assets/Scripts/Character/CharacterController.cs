using UnityEngine;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler    {


    public AudioClip NotificationSound;
    public bool isGrounded = true;

    private AudioSource audioSource;

    private bool isFacingRight = false;
    private float velocityHorizontal = 0f;
    private float velocityVertical = 0f;
    public GameObject spritesheet;
    private Animator animator;
    private GameObject player;
    private Rigidbody2D body;
    private Character character;
    private GameObject trigger;

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
        body.velocity = new Vector2(velocityHorizontal * character.speed, body.velocity.y);
    }

    public override void onSwimming()
    {

        StopCoroutine("underWaterBreath");

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

    public override void onMovingPlatform()
    {
        body.gravityScale = 150;
        body.mass = 10;
        if (velocityHorizontal == 0)
        {
            
            body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((character.speed / transform.parent.GetComponent<Rigidbody2D>().velocity.x) * velocityHorizontal), body.velocity.y);

        }
    }

    public override void onFalling()
    {
        body.gravityScale = 60;
        body.mass = 10;
       
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
        
            
    }

    public override void onUnderWater()
    {
        StartCoroutine("underWaterBreath");
        body.gravityScale = 80;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * (character.speed / 4f), velocityVertical * (character.speed / 4f));
    }

    public override void onDeath()
    {
        body.velocity = Vector3.zero;
        body.position = new Vector2(57.4f, -647.5f);
        character.health = 100;
    }








    void Start()
    {
        base.Start();
        character = new Character();
        character.health = 100;
        character.jump = 450;
        character.speed = 160;
        print(character.health);

        player = this.gameObject;
        animator = gameObject.GetComponent<Animator>();
        body = player.AddComponent<Rigidbody2D>();
        audioSource = player.AddComponent<AudioSource>();

        //Setting up player trigger
        /*
        trigger = new GameObject();
        trigger.name = "trigger";
        trigger.tag = "Player";
        trigger.gameObject.transform.parent = GameObject.Find("Player").transform;
        trigger.AddComponent<Rigidbody2D>();
        Rigidbody2D triggerBody = (Rigidbody2D)trigger.GetComponent<Rigidbody2D>();
        triggerBody.gravityScale = 0; 
        
        trigger.AddComponent<BoxCollider2D>();
        BoxCollider2D triggerCol = (BoxCollider2D)trigger.GetComponent<BoxCollider2D>();
        triggerCol.isTrigger = true;
        triggerCol.size = new Vector2(5, 15);
        */
        //trigger.

        body.isKinematic = false;
        body.fixedAngle = true;
    }
    

    void Update()
    {
        base.Update();

        movement();
        checkRespawnHeight(-3000);
        print("health" + character.health);
        if (character.health == 0)
        {
            state = State.STATE_DEATH;
        }

        //trigger.gameObject.transform.position = this.gameObject.transform.position;
        //print(state);
    }

    private void checkRespawnHeight(int height)
    {
        if (transform.position.y < height)
        {
            body.velocity = Vector3.zero;
            body.position = new Vector2(57.4f, -647.5f);
        }
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

        if (isGrounded)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
            {
                jump();
            }
        }
        
    }

    private void jump()
    {
        if (state != State.STATE_CLIMBING && state != State.STATE_FALLING)
        {
            body.velocity = new Vector2(body.velocity.x, character.jump);
        }
        
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
        if (coll.tag.ToString().Equals("Ladder"))
        {
            state = State.STATE_CLIMBING;
            animator.SetBool("Climbing", true);
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            state = State.STATE_SWIMMING;
        }
        else if (coll.tag.ToString().Equals("UnderWater"))
        {
            state = State.STATE_UNDERWATER;
            print("DEAD");
        }

    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Ladder"))
        {
            state = State.STATE_CLIMBING;
            animator.SetBool("Climbing", true);
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            state = State.STATE_SWIMMING;
        }
        else if (coll.tag.ToString().Equals("UnderWater"))
        {
            state = State.STATE_UNDERWATER;
            print("DEAD");
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Ladder"))
        {
            state = State.STATE_IDLE;
            animator.SetBool("Climbing", false);
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            state = State.STATE_IDLE;
        }
        else if (coll.tag.ToString().Equals("UnderWater"))
        {
            state = State.STATE_SWIMMING;
            print("You may have saved yourself.....");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            if (coll.transform.tag.ToString().Equals("MovingPlatform"))
            {
                state = State.STATE_MOVINGPLATFORM;
                isGrounded = true;
            }
            else if (coll.transform.tag.ToString().Equals("Ground"))
            {

                state = State.STATE_IDLE;
                isGrounded = true;
                animator.SetBool("Jumping", false);


            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            if (coll.transform.tag.ToString().Equals("MovingPlatform"))
            {
                state = State.STATE_MOVINGPLATFORM;
                isGrounded = true;
                animator.SetBool("Jumping", false);
            }
            else if (coll.transform.tag.ToString().Equals("Ground"))
            {

                isGrounded = true;
                animator.SetBool("Jumping", false);

            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            if (coll.transform.tag.ToString().Equals("MovingPlatform"))
            {
                state = State.STATE_IDLE;
                isGrounded = false;

                animator.SetBool("Jumping", true);
            }
            else if (coll.transform.tag.ToString().Equals("Ground"))
            {

                isGrounded = false;
                state = State.STATE_JUMPING;
                animator.SetBool("Jumping", true);


            }
        }
        
    }



    private bool checkIfSide(Collision2D coll)
    {
        //print (coll.contacts.Length);
        //print(coll.contacts[0].normal.y);

        if (coll.contacts[0].normal.y == 0)
        {
            if (state == State.STATE_JUMPING || state == State.STATE_WALKING)
            {
                state = State.STATE_FALLING;
                isGrounded = false;
                return true;
            }
            return false;
            
        }
        else
        {
            if (state == State.STATE_FALLING)
            {
                state = State.STATE_IDLE;
            }
            
            return false;
        }

        
    }



    IEnumerator underWaterBreath()
    {
        yield return new WaitForSeconds(2);
        character.health  = character.health - 25;
        print("ow");
    }

}
