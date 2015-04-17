using UnityEngine;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler
{


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
    public Character character;
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
        if (velocityHorizontal == 0)
        {
            state = State.STATE_IDLE;
        }

        if (isGrounded)
        {
            body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * character.speed, body.velocity.y);
        }
        else
        {
            body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * character.speed, body.velocity.y);
        }

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

    public override void onStairs()
    {
        body.gravityScale = 0;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * character.speed, velocityVertical * (character.speed / 2));
    }

    public override void onCrossJunction()
    {
        //isGrounded = true;
        body.gravityScale = 0;
        body.mass = 10;

        body.velocity = new Vector2(velocityHorizontal * character.speed, velocityVertical * (character.speed / 2));
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
        body.isKinematic = false;
        body.fixedAngle = true;
    }


    void Update()
    {
        base.Update();

        movement();
        checkRespawnHeight(-3000);
        //print("health" + character.health);
        if (character.health == 0)
        {
            state = State.STATE_DEATH;
        }

        if (isGrounded)
        {
            animator.SetBool("Jumping", false);
        }
        else
        {
            animator.SetBool("Jumping", true);
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
            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space"))
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
            StartCoroutine("underWaterBreath");
        }
        else if (coll.tag.ToString().Equals("Stairs"))
        {

            state = State.STATE_STAIRS;


        }
        else if (coll.tag.ToString().Equals("CrossJunction"))
        {
            if (state == State.STATE_WALKING)
            {
                state = State.STATE_WALKING;
            }

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
        }
        else if (coll.tag.ToString().Equals("Stairs") || coll.tag.ToString().Equals("Stairs_top"))
        {
            
            state = State.STATE_STAIRS;
            //animator.SetBool("Jumping", false);

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
            state = State.STATE_IDLE;
            StopCoroutine("underWaterBreath");
        }
        else if (coll.tag.ToString().Equals("Stairs"))
        {
            state = State.STATE_IDLE;
        } 
        else if (coll.tag.ToString().Equals("Stairs_top"))
        {
            state = State.STATE_IDLE;
            coll.transform.GetComponent<Collider2D>().isTrigger = false;
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
            else if (coll.transform.tag.ToString().Equals("Enemy"))
            {
                int attack = coll.gameObject.GetComponent<EnemyController>().enemy.attackPower;

                character.health -= attack;
                
                
            }
            else
            {
                //isGrounded = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            //print(coll.transform.tag.ToString());
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
            else if (coll.transform.tag.ToString().Equals("Enemy"))
            {
                int attack = coll.gameObject.GetComponent<EnemyController>().enemy.attackPower;


                if (coll.gameObject.GetComponent<EnemyController>().state.Equals("STATE_ATTACKING"))
                {
                    print("ow");
                    character.health -= attack;
                }

            }
            else if (coll.transform.tag.ToString().Equals("Stairs"))  
            { 
                state = State.STATE_STAIRS;
            }
            else if (coll.transform.tag.ToString().Equals("Stairs_top"))
            {
                if (velocityVertical < -0.7 || velocityVertical > 0.7)
                {
                    coll.gameObject.GetComponent<Collider2D>().isTrigger = true;
                    state = State.STATE_STAIRS;
                    enableStairs(true);
                }
                else
                {
                    coll.gameObject.GetComponent<Collider2D>().isTrigger = false;
                    state = State.STATE_IDLE;
                    enableStairs(false);
                }
            }
            else
            {
                isGrounded = false;
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
                //state = State.STATE_JUMPING;
                animator.SetBool("Jumping", true);


            }
            else if (coll.transform.tag.ToString().Equals("Stairs_top"))
            {
                GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stairs");
                for (int i = 0; i < stairs.Length; i++)
                {
                    if (stairs[i].transform.GetComponent<Collider2D>())
                    {
                        stairs[i].transform.GetComponent<Collider2D>().enabled = true;
                    }

                    //GameObject collision = coll.gameObject.transform.FindChild("Collision").gameObject;
                }
            }
        }

    }


    private bool checkIfSide(Collision2D coll)
    {
        if (!isGrounded)
        {
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
        return false;


    }



    IEnumerator underWaterBreath()
    {
        yield return new WaitForSeconds(2);
        character.health = character.health - 25;
        StartCoroutine("underWaterBreath");
    }

    //from http://forum.unity3d.com/threads/trigger-collision-contact-point.26232/
    private bool ifAirAbove()
    {
        if (Physics2D.Raycast(transform.position, transform.up, -60).collider.tag == "Stairs")
        {
            print("hit");
            return true;

        }
        return false;
    }

    private void enableStairs(bool enable)
    {
        GameObject stairs = GameObject.FindGameObjectWithTag("Stairs");
        if (enable)
        {
            stairs.transform.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            stairs.transform.GetComponent<Collider2D>().enabled = false;
            isGrounded = true;
        }
    }


}
