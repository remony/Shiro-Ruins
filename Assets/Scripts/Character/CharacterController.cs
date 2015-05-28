using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler
{


    public AudioClip NotificationSound;
    public bool isGrounded = true;

    private GameObject levelManager;

    private bool musicPlaying = false;
    public float currentVelocity = 0;
    public bool isFacingRight = false;
    private float velocityHorizontal = 0f;
    private float velocityVertical = 0f;
    public GameObject spritesheet;
    private Animator animator;
    private GameObject player;
    private Rigidbody2D body;
    public Character character;
    private GameObject trigger;
    private Vector2 boxCol;
    public int respawnHeight;

    private Transform previousPostiton; //used to stop hugging walls


    private bool moveJump = false;

    public float magicCooldown = 1;

    public GameObject blastPrefab;
    public GameObject magicBlastPrefab;

    public override void onIdle()
    {

        body.gravityScale = 100;
        if (velocityHorizontal != 0)
        {
            currentVelocity = character.speed;
        }

        body.mass = 1;
        if (!isGrounded)
        {
            state = State.STATE_JUMPING;
        }
        if (velocityHorizontal > 0.1 || velocityHorizontal < -0.1)
        {
            state = State.STATE_WALKING;
        }
             
    }

    public override void onWalking()
    {
        body.gravityScale = 50;
        body.mass = 100;
        if (velocityHorizontal == 0)
        {
            state = State.STATE_IDLE;
        }

        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }


        float timez = 0.1f;

        if (currentVelocity < 200)
        {
            timez = 1;
        } 
        else if (currentVelocity > 150 && currentVelocity < 240)
        {
            timez = 0.5f;
        }
        else if(currentVelocity > 200 && currentVelocity < 250)
        {
            timez = 0.25f;
        }
        else if(currentVelocity > 250 && currentVelocity < 300)
        {
            timez = 0.01f;
        }
        

        //calculate the speed for movement
        if (velocityHorizontal < 0)
        {
            currentVelocity += character.speed * ((Time.deltaTime * -velocityHorizontal)* timez);
        }
        else if (velocityHorizontal > 0)
        {
            currentVelocity += character.speed * ((Time.deltaTime * velocityHorizontal) * timez);
        }

        
        
        
        if (isGrounded)
        {
            //body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
        }
        else
        {
            state = State.STATE_JUMPING;
            //body.gravityScale = 150;
            body.velocity = new Vector2(velocityHorizontal * currentVelocity, body.velocity.y);
        }

    }

    public override void onJumping()
    {
        body.gravityScale = 100;
        body.mass = 1;

        if (isGrounded)
        {
            state = State.STATE_WALKING;
        }

        if (currentVelocity > 300)
        {
            currentVelocity = 300;
        }
        body.velocity = new Vector2(velocityHorizontal * currentVelocity *0.9f, body.velocity.y);
        

        
        
        //body.AddForce(new Vector2(velocityHorizontal * currentVelocity, body.velocity.y), ForceMode2D.Impulse);
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
        body.gravityScale = 100;
        body.mass = 1;
        if (velocityHorizontal == 0)
        {
            if (transform.parent)
            {
                body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, body.velocity.y);
            }

           
        }
        else
        {
            if (transform.parent)
            {
                body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((currentVelocity / transform.parent.GetComponent<Rigidbody2D>().velocity.x) * velocityHorizontal), body.velocity.y);
            }
        }
    }

    public override void onFalling()
    {
        body.gravityScale = 180;
        body.mass = 10;

 
        if (isFacingRight)
        {
            body.velocity = new Vector2((character.speed * velocityHorizontal), body.velocity.y);
        }
        else if (!isFacingRight)
        {
            body.velocity = new Vector2((character.speed * velocityHorizontal), body.velocity.y);
        }
        
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
        body.gravityScale = 300;
        body.mass = 1000;
        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }
        body.velocity = new Vector2(0, velocityVertical * character.speed);
        
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

        body.gravityScale = 100;
        body.mass = 1;
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
        character.health -= 50;
        hurt();
        StartCoroutine("lavaDamage", 0.25f);
    }

    new
    void Start()
    {
        base.Start();
        character = new Character();
        //If the current game mode is hardware then set the character to 17
        try
        {
            if (GameManager.instance.GetGameType() == 2)
            {
                character.health = 1;
            }
            else //For every other gamemode set the character health to full (100)
            {
                character.health = 100;
            }
        }
        catch (Exception e)
        {
            print(e);
            character.health = 100;
        }

        if (respawnHeight == 0)
        {
            respawnHeight = -3000;
        }
        

        
        character.jump = 390;
        character.speed = 120;
        character.cooldownLimit = magicCooldown;
        character.blastCooldownLimit = (magicCooldown * 6f);
        character.startingPosition = new Vector2(transform.position.x, transform.position.y);


        player = this.gameObject;
        animator = gameObject.GetComponent<Animator>();
        body = player.AddComponent<Rigidbody2D>();
        body.isKinematic = false;
        body.fixedAngle = true;
        boxCol = body.GetComponent<BoxCollider2D>().size;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
    }


    new
    void Update()
    {
        base.Update();
        playerInput();
        movement();

        checkRespawnHeight(respawnHeight); //If the player is below the fall limit then they will respawn to the start of the map.
   

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
            
            
        }

        levelManager.GetComponent<GuiObserver>().changeGrounded(isGrounded);

    }

    private void playerInput()
    {

        if (velocityHorizontal >= -0.1 && velocityHorizontal <= 0.1f)
        {
            currentVelocity = character.speed;
        }


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



        if (character.blastCooldownTimer >= character.blastCooldownLimit)
        {
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown("joystick button 1"))
            {
                print("L");
                if (character.blastCooldownTimer < character.blastCooldownLimit)
                {
                    print("poof cooldown");
                }
                else
                {
                    character.blastCooldownStart = Time.time;
                    character.blastCooldownTimer = Time.time - character.blastCooldownStart;
                    shotBlast();
                    print("boom");
                }
            }
        }
        else
        {
            if (character.blastCooldownStart == 0)
            {
                character.blastCooldownStart = -character.blastCooldownLimit;
            }
            // (time * 100 / 3)

            character.blastCooldownTimer = Time.time - character.blastCooldownStart;
        }

        levelManager.GetComponent<GuiObserver>().updateSphereCooldown(character.blastCooldownTimer / character.blastCooldownLimit);

        
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
            character.cooldownTimer = Time.time - character.cooldownStart;

        }
        levelManager.GetComponent<GuiObserver>().updateMagicCooldown(character.cooldownTimer / character.cooldownLimit);

    }

    private void shotMagic()
    {
        GameObject bullet = GameObject.Instantiate(magicBlastPrefab, new Vector2(transform.position.x + 0, transform.position.y), transform.rotation) as GameObject;

        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 1)
        {
            GameManager.instance.playSoundEffect(8);
        }
        else
        {
            GameManager.instance.playSoundEffect(9);

        }
        
        if (isFacingRight)
        {
            bullet.GetComponent<MagicBulletController>().isRight = true;
        }
        else
        {
            bullet.GetComponent<MagicBulletController>().isRight = false;
        }
    }

    private void shotBlast()
    {
        GameObject blast = GameObject.Instantiate(blastPrefab, new Vector2(transform.position.x + 0, transform.position.y), transform.rotation) as GameObject;
        GameManager.instance.playSoundEffect(7);
   
    }

    private void respawn()
    {
        body.velocity = Vector3.zero;
        gameObject.transform.position = character.startingPosition;
        //print(character.startingPosition.position.x + "  " + character.startingPosition.position.y);
    }

    private void checkRespawnHeight(int height)
    {
        if (transform.position.y < height)
        {
            body.velocity = Vector3.zero;
            GameManager.instance.playSoundEffect(1);
            transform.position = character.startingPosition;//new Vector2(57.4f, -1347.5f);
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
            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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
            int rand = UnityEngine.Random.Range(0, 2);
            if (rand == 1)
            {
                GameManager.instance.playSoundEffect(5);
            }
            else
            {
                GameManager.instance.playSoundEffect(6);

            }
            isGrounded = false;
            
        }

    }

    private void flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;
        if (state.Equals(State.STATE_WALKING))
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
            StartCoroutine("lavaDamage", 0.25f);
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
            hurt();
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
            state = State.STATE_WALKING;
        } 
        else if (coll.tag.ToString().Equals("Stairs_top"))
        {
            state = State.STATE_WALKING;
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
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platforms") || coll.transform.tag.ToString().Equals("Platform") || coll.transform.tag.ToString().Equals("BreakingPlatform"))
            {
                if (!state.Equals(State.STATE_UNDERWATER))
                {
                    if (checkIfTop(coll))
                    {
                        body.velocity = new Vector2(0, 0);
                        isGrounded = true;
                        state = State.STATE_WALKING;
                        animator.SetBool("Jumping", false);

                    }
                    else if (checkIfBottom(coll))
                    {
                        state = State.STATE_FALLING;
                        isGrounded = false;
                    }
                    else
                    {
                        state = State.STATE_WALKING;
                        isGrounded = true;
                    }
                }
                

            }
            else if (coll.transform.tag.ToString().Equals("StairsPlatform"))
            {
                isGrounded = true;
                state = State.STATE_WALKING;
                animator.SetBool("Jumping", false);
            }
            /*
        else if (coll.transform.tag.ToString().Equals("Enemy"))
        {
                
            //int attack = coll.gameObject.GetComponent<EnemyController>().enemy.attackPower;

            //character.health -= attack;


        }*/
            else if (coll.transform.tag.ToString().Equals("End"))
            {
                //state = State.STATE_NOGRAVITY;
                atEndGoal();
                isGrounded = true;
            }
            else if (coll.transform.tag.ToString().Equals("Untagged"))
            {
                if (checkIfBottom(coll))
                    isGrounded = false;
            }
            else if (coll.transform.tag.ToString().Equals("WaterPlatform"))
            {
                currentVelocity = currentVelocity * 0.8f;
                isGrounded = true;
            }
            else
            {
                isGrounded = true;
            }
        }
        else
        {
            //state = State.STATE_FALLING;
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
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platforms") || coll.transform.tag.ToString().Equals("Platform") || coll.transform.tag.ToString().Equals("BreakingPlatform"))
            {
                if (checkIfTop(coll))
                {
                    isGrounded = true;
                    state = State.STATE_WALKING;
                    animator.SetBool("Jumping", false);

                }
                


                if (coll.contacts[0].normal == new Vector2(0, 1))
                {
                    //isGrounded = true;
                    animator.SetBool("Jumping", false);

                }
            }
            else if (coll.transform.tag.ToString().Equals("StairsPlatform"))
            {
                isGrounded = true;
            }
            else if (coll.transform.tag.ToString().Equals("Enemy"))
            {
                

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
            else if (coll.transform.tag.ToString().Equals("WaterPlatform"))
            {
                //currentVelocity = currentVelocity - 20 ;
            }

            else
            {
                //isGrounded = true;
            }
        }
        else
        {
            //state = State.STATE_FALLING;
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
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platforms") || coll.transform.tag.ToString().Equals("Platform"))
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
            else if (coll.transform.tag.ToString().Equals("BreakingPlatform"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

    }

    //check that you are hitting the top of an object
    private bool checkIfTop(Collision2D coll)
    {
        if (coll.contacts[0].normal == new Vector2(0, 1))
        {
            return true;
        }
        return false;
    }


    //check that you are hitting the bottom of an object
    private bool checkIfBottom(Collision2D coll)
    {
        if (coll.contacts[0].normal == new Vector2(0, -1))
        {
            return true;
        }
        return false;
    }

    private bool checkIfSide(Collision2D coll)
    {
        if (coll.contacts[0].normal == new Vector2(-1, 0)) //left
        {
            return true;
        }

        if (coll.contacts[0].normal == new Vector2(1, 0)) //right
        {
            return true;
        }

        return false;
        /*
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
        */

    }



    IEnumerator underWaterBreath()
    {
        yield return new WaitForSeconds(2);
        character.health = character.health - 25;
        hurt();
        StartCoroutine("underWaterBreath");
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            respawn();
        }

        


        //if (velocityHorizontal < 0.1 && velocityHorizontal > -0.1)
        //{
         //   if (state != State.STATE_JUMPING)
          //      state = State.STATE_IDLE;
        //} 
        //else
        //{
         //   state = State.STATE_WALKING;
        //}


        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }

        levelManager.GetComponent<GuiObserver>().UpdateHealth(character.health);
    }

    public void heal(int health)
    {
        
        character.health += health;
        if (character.health > 100)
        {
            character.health = 100;
        }
    }


    public void takeDamage(int amount)
    {
        character.health -= amount;
        hurt();
    }

    private void hurt()
    {
        if (character.health > 0)
            GameManager.instance.playSoundEffect(0);
    }

}
