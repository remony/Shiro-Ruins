using UnityEngine;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler
{


    public AudioClip NotificationSound;
    public bool isGrounded = true;

    private GameObject levelManager;
        
    private AudioSource audioSource;
    private float currentVelocity = 0;
    private bool isFacingRight = false;
    private float velocityHorizontal = 0f;
    private float velocityVertical = 0f;
    public GameObject spritesheet;
    private Animator animator;
    private GameObject player;
    private Rigidbody2D body;
    public Character character;
    private GameObject trigger;
    private Vector2 boxCol;

    public float magicCooldown = 1;

    public GameObject magicBlastPrefab;

    public override void onIdle()
    {

        body.gravityScale = 150;
        if (velocityHorizontal != 0)
        {
            currentVelocity = character.speed;
        }

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
        if (body.velocity.x == 0)
        {
            state = State.STATE_IDLE;
        }

        //calculate the speed for movement
        if (velocityHorizontal < 0)
        {
            currentVelocity += character.speed * (Time.deltaTime * -velocityHorizontal);
        }
        else if (velocityHorizontal > 0)
        {
            currentVelocity += character.speed * (Time.deltaTime * velocityHorizontal);
        }

        
        
        
        if (isGrounded)
        {
            body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
        }
        else
        {
            body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
        }

    }

    public override void onJumping()
    {
        body.gravityScale = 150;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
    }

    public override void onSwimming()
    {

        StopCoroutine("underWaterBreath");

        body.gravityScale = 80;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * (currentVelocity / 4f), velocityVertical * (character.speed / 4f));
    }

    public override void onClimbing()
    {
        body.gravityScale = 0;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * currentVelocity, velocityVertical * character.speed);
        animator.SetBool("Climbing", true);

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
            body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((currentVelocity / transform.parent.GetComponent<Rigidbody2D>().velocity.x) * velocityHorizontal), body.velocity.y);

        }
    }

    public override void onFalling()
    {
        body.gravityScale = 60;
        body.mass = 10;

        body.velocity = new Vector2(body.velocity.x, body.velocity.y);
        if (isGrounded)
        {
            state = State.STATE_IDLE;
        }

    }

    public override void onUnderWater()
    {
        body.gravityScale = 80;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * (currentVelocity / 4f), velocityVertical * (character.speed / 4f));
    }

    public override void onLava()
    {
        body.gravityScale = 80;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * (currentVelocity / 4f), velocityVertical * (character.speed / 4f));
        
    }

    public override void onDeath()
    {
        body.velocity = Vector3.zero;
        levelManager.GetComponent<GuiObserver>().ChangeState(4);

    }

    public override void onStairs()
    {
        body.gravityScale = 0;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * currentVelocity, velocityVertical * (character.speed * 0.6f));
    }

    public override void onCrossJunction()
    {
        //isGrounded = true;
        body.gravityScale = 0;
        body.mass = 10;

        body.velocity = new Vector2(velocityHorizontal * currentVelocity, velocityVertical * (character.speed / 2));
    }

    public override void onNoGravity()
    {
        body.gravityScale = 10;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.transform.position.y);
    }

    public override void onAttacking()
    {

        body.gravityScale = 150;
        body.mass = 10;
        body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
        
        body.GetComponent<BoxCollider2D>().size = new Vector2(boxCol.x + 0.2f, boxCol.y);
    }



    IEnumerator attack(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.STATE_IDLE;
        animator.SetBool("Attacking", false);
        body.GetComponent<BoxCollider2D>().size = boxCol;
    }

    IEnumerator lavaDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        character.health -= 25;
        StartCoroutine("lavaDamage", 0.25f);
    }


    void Start()
    {
        base.Start();
        character = new Character();
        character.health = 100;
        character.jump = 450;
        character.speed = 75;
        //print(character.health);

        character.cooldownLimit = magicCooldown;



        player = this.gameObject;
        animator = gameObject.GetComponent<Animator>();
        body = player.AddComponent<Rigidbody2D>();
        audioSource = player.AddComponent<AudioSource>();
        body.isKinematic = false;
        body.fixedAngle = true;
        boxCol = body.GetComponent<BoxCollider2D>().size;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
    }


    
    void Update()
    {
        base.Update();
        playerInput();
        movement();

        checkRespawnHeight(-3000);

        

        

        if (isGrounded)
        {
            animator.SetBool("Jumping", false);
        }
        else
        {
            animator.SetBool("Jumping", true);
            isGrounded = false;
            if (character.health <= 0)
            {
                state = State.STATE_DEATH;
            }
            else
            {
                state = State.STATE_JUMPING;
            }
            
        }

        levelManager.GetComponent<GuiObserver>().changeGrounded(isGrounded);

    }

    private void playerInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            state = State.STATE_ATTACKING;
            StartCoroutine("attack", 0.5f);
            animator.SetBool("Attacking", true);
            
        }

        if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown("q"))
        {
            levelManager.GetComponent<GuiObserver>().displayLastMessage();
        }
        
        if (character.cooldownTimer >= character.cooldownLimit)
        {
            //character.cooldownTimer = 0;
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown("joystick button 2"))
            {


                if (character.cooldownTimer < character.cooldownLimit)
                {
                    //Play sound
                    print("Poof: cooldown");
                }
                else
                {
                    //set start time
                    //character.cooldownTimer = Time.time - character.cooldownTimer;
                    character.cooldownStart = Time.time;
                    character.cooldownTimer = Time.time - character.cooldownStart;
                    shotMagic();

                }


            }
        } 
        else
        {
            if (character.cooldownStart == 0)
            {
                character.cooldownStart = -character.cooldownLimit;
            }
            // (time * 100 / 3)
            
            character.cooldownTimer = Time.time - character.cooldownStart;

        }
        levelManager.GetComponent<GuiObserver>().updateMagicCooldown(character.cooldownTimer / character.cooldownLimit);
        
        //print("cooldown: " + character.cooldownTimer + "| start: " + character.cooldownStart + " limited to " + character.cooldownLimit);

    }

    private void shotMagic()
    {
        GameObject bullet = GameObject.Instantiate(magicBlastPrefab, new Vector2(transform.position.x + 0, transform.position.y), transform.rotation) as GameObject;
        if (isFacingRight)
        {
            bullet.GetComponent<MagicBulletController>().isRight = true;
        }
        else
        {
            bullet.GetComponent<MagicBulletController>().isRight = false;
        }
        
        
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
            if (state == State.STATE_WALKING)
            {
                body.velocity = new Vector2(body.velocity.x * 2, character.jump);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x, character.jump);
            }
            isGrounded = false;
            
        }

    }

    private void flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;
        currentVelocity = character.speed;
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
        else if (coll.tag.ToString().Equals("Lava"))
        {
            state = State.STATE_LAVA;
            StartCoroutine("lavaDamage", 1);
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
        else if (coll.tag.ToString().Equals("Lava"))
        {
            state = State.STATE_LAVA;
        }
        else if (coll.tag.ToString().Equals("Spike"))
        {
            character.health = 0;
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
            //state = State.STATE_IDLE;
        } 
        else if (coll.tag.ToString().Equals("Stairs_top"))
        {
            //state = State.STATE_IDLE;
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
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platform"))
            {
                isGrounded = true;
                animator.SetBool("Jumping", false);
            }   
            else if (coll.transform.tag.ToString().Equals("Enemy"))
            {
                int attack = coll.gameObject.GetComponent<EnemyController>().enemy.attackPower;

                //character.health -= attack;


            }
            else if (coll.transform.tag.ToString().Equals("End"))
            {
                //state = State.STATE_NOGRAVITY;
                atEndGoal();
                isGrounded = true;
            }
            else
            {
                isGrounded = true;
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
                    //character.health -= attack;
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
                    //state = State.STATE_IDLE;
                    enableStairs(false);
                }
            }
            else if (coll.transform.tag.ToString().Equals("Platform"))
            {
                animator.SetBool("Jumping", false);
                state = State.STATE_WALKING;
                isGrounded = true;
            }
            else
            {
                isGrounded = true;
            }
        }
    }

    

    void OnCollisionExit2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            if (coll.transform.tag.ToString().Equals("MovingPlatform"))
            {
                //state = State.STATE_IDLE;
                isGrounded = false;

                animator.SetBool("Jumping", true);
            }
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platform"))
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
            else if (coll.transform.tag.ToString().Equals("Stairs"))
            {
                animator.SetBool("Climbing", false);
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
        //print(">> " + enable);
        GameObject stairs = GameObject.FindGameObjectWithTag("Stairs");
        if (enable)
        {

            if (stairs.transform.GetComponent<Collider2D>())
            {
                stairs.transform.GetComponent<Collider2D>().enabled = true;
            }
        }
        else if (!enable)
        {
            if (stairs.transform.GetComponent<Collider2D>())
            {
                stairs.transform.GetComponent<Collider2D>().enabled = false;
            }
            
            isGrounded = true;
        }
    }


    //Player is at the end goal of the level, tell the levelmanager
    private void atEndGoal()
    {
        levelManager.GetComponent<GuiObserver>().ChangeState(3);
    }



    void FixedUpdate()
    {
        if (currentVelocity >= 300)
        {
            currentVelocity = 300;
        }

        if (velocityHorizontal == 0)
        {
            state = State.STATE_IDLE;
        } 
        else
        {
            state = State.STATE_WALKING;
        }


        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }

        levelManager.GetComponent<GuiObserver>().UpdateHealth(character.health);
    }

}
