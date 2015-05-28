using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class CharacterController : CharacterStateHandler
{


    public AudioClip NotificationSound;
    public bool isGrounded = true;   
    public float currentVelocity = 0;
    public bool isFacingRight = false;
    public Character character;
    private Vector2 boxCol;
    public int respawnHeight;
    public float magicCooldown = 1;

    private GameObject levelManager;
    public GameObject blastPrefab;
    public GameObject magicBlastPrefab;
    public GameObject spritesheet;

    //When the player is not moving it is in idle state
    public override void onIdle()
    {

        character.body.gravityScale = 100;
        if (character.velocityHorizontal != 0)
        {
            currentVelocity = character.speed;
        }

        character.body.mass = 1;
        if (!isGrounded) //If the player is in idle and jumps it will change to jumping state
        {
            state = State.STATE_JUMPING;
        }
        if (character.velocityHorizontal > 0.1 || character.velocityHorizontal < -0.1) //if horizontal input is detected from devices change the state to walking
        {
            state = State.STATE_WALKING;
        }

        if (character.health <= 0) //If health is < or equal to 0 change state dealth
        {
            state = State.STATE_DEATH;
        }
             
    }

    //When the player is in walking state
    public override void onWalking()
    {
        character.body.gravityScale = 50;
        character.body.mass = 100;
        if (character.velocityHorizontal == 0) //If there is no movement input from the player set character to idle
        {
            state = State.STATE_IDLE;
        }

        if (character.health <= 0) //If health is < or equal to 0 change state dealth
        {
            state = State.STATE_DEATH;
        }


        float TimeIncrease = 0.1f;

        if (currentVelocity < 200)
        {
            TimeIncrease = 1;
        } 
        else if (currentVelocity > 150 && currentVelocity < 240)
        {
            TimeIncrease = 0.5f;
        }
        else if(currentVelocity > 200 && currentVelocity < 250)
        {
            TimeIncrease = 0.25f;
        }
        else if(currentVelocity > 250 && currentVelocity < 300)
        {
            TimeIncrease = 0.01f;
        }
        

        //calculate the speed for movement
        if (character.velocityHorizontal < 0)
        {
            currentVelocity += character.speed * ((Time.deltaTime * -character.velocityHorizontal) * TimeIncrease);
        }
        else if (character.velocityHorizontal > 0)
        {
            currentVelocity += character.speed * ((Time.deltaTime * character.velocityHorizontal) * TimeIncrease);
        }

        
        
        //Apply velocity to the character using the calculated speed
        if (isGrounded)
        {
            character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.body.velocity.y);
        }
        else
        {
            state = State.STATE_JUMPING;
            character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.body.velocity.y);
        }

    }


    //When in jumping state
    public override void onJumping()
    {
        character.body.gravityScale = 100;
        character.body.mass = 1;

        if (isGrounded)
        {
            state = State.STATE_WALKING;
        }

        if (currentVelocity > 300)
        {
            currentVelocity = 300;
        }
        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity * 0.9f, character.body.velocity.y);
    }

    //When the player is swimming
    public override void onSwimming()
    {

        StopCoroutine("underWaterBreath");

        character.body.gravityScale = 80;
        character.body.mass = 10;
        //movement is slower underwater 
        character.body.velocity = new Vector2(character.velocityHorizontal * (currentVelocity / 2f), character.velocityVertical * (character.speed / 2f));
    }


    //When the player is climbing
    public override void onClimbing()
    {
        character.body.gravityScale = 0; //disable gravity for the player
        character.body.mass = 10;
        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.velocityVertical * character.speed); //slow down movement a bit
        character.animator.SetBool("Climbing", true);

    }

    //When the player is on a moving platform
    public override void onMovingPlatform()
    {
        character.body.gravityScale = 100;
        character.body.mass = 1;
        if (character.velocityHorizontal == 0)
        {
            if (transform.parent)
            {
                //Set the velocity of the player to that of the moving platforma
                character.body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, character.body.velocity.y);
            }

           
        }
        else
        {
            if (transform.parent)
            {
                //Set the velocity of the player to that of the moving platform and that of your own movement speed (this is for moving on the platform)
                character.body.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((currentVelocity / transform.parent.GetComponent<Rigidbody2D>().velocity.x) * character.velocityHorizontal), character.body.velocity.y);
            }
        }
    }

    //When the player is falling
    public override void onFalling()
    {
        character.body.gravityScale = 180;
        character.body.mass = 10;

 
        if (isFacingRight)
        {
            character.body.velocity = new Vector2((character.speed * character.velocityHorizontal), character.body.velocity.y);
        }
        else if (!isFacingRight)
        {
            character.body.velocity = new Vector2((character.speed * character.velocityHorizontal), character.body.velocity.y);
        }
        
        if (isGrounded)
        {
            state = State.STATE_IDLE;
        }

    }

    //When the player is underwater
    public override void onUnderWater()
    {
        character.body.gravityScale = 80;
        character.body.mass = 10;
        character.body.velocity = new Vector2(character.velocityHorizontal * (currentVelocity / 4f), character.velocityVertical * (character.speed / 4f));
    }

    //When the player is in lava
    public override void onLava()
    {
        character.body.gravityScale = 300;
        character.body.mass = 1000;
        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }
        //Restrict all movement
        character.body.velocity = new Vector2(0, character.velocityVertical * character.speed);
        
    }

    //When the player is dead
    public override void onDeath()
    {
        character.body.velocity = Vector3.zero;
        levelManager.GetComponent<GuiObserver>().ChangeState(4); //change the state of the GUI to display the failed screen

    }

    public override void onStairs()
    {
        character.body.gravityScale = 0;
        character.body.mass = 10;
        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.velocityVertical * (character.speed * 0.6f));
    }

    public override void onCrossJunction()
    {
        //isGrounded = true;
        character.body.gravityScale = 0;
        character.body.mass = 10;

        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.velocityVertical * (character.speed / 2));
    }

    public override void onNoGravity()
    {
        character.body.gravityScale = 10;
        character.body.mass = 10;
        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.body.transform.position.y);
    }

    public override void onAttacking()
    {

        character.body.gravityScale = 100;
        character.body.mass = 1;
        character.body.velocity = new Vector2(character.velocityHorizontal * currentVelocity, character.body.velocity.y);

        character.body.GetComponent<BoxCollider2D>().size = new Vector2(boxCol.x + 0.2f, boxCol.y);
    }


    //attack  wait for x seconds then change the animation to attacking and change the box collider to cover the new area
    IEnumerator attack(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.STATE_IDLE;
        character.animator.SetBool("Attacking", false);
        character.body.GetComponent<BoxCollider2D>().size = boxCol;
    }

    //Lava damage deals damage over x seconds
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
        character.player = this.gameObject;
        character.animator = gameObject.GetComponent<Animator>();
        character.body = character.player.AddComponent<Rigidbody2D>();
        character.body.isKinematic = false;
        character.body.fixedAngle = true;
        character.moveJump = false;
        character.velocityHorizontal = 0f;
        character.velocityVertical = 0f;
        character.musicPlaying = false;

        boxCol = character.body.GetComponent<BoxCollider2D>().size;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");

    }


    new
    void Update()
    {
        base.Update();
        playerInput(); //Handles the player input
        movement();

        checkRespawnHeight(respawnHeight); //If the player is below the fall limit then they will respawn to the start of the map.
   

        if (isGrounded)
        {
            character.animator.SetBool("Jumping", false);
        }
        else
        {
            character.animator.SetBool("Jumping", true);
            isGrounded = false;
            if (character.health <= 0)
            {
                state = State.STATE_DEATH;
            }
            
            
        }

        levelManager.GetComponent<GuiObserver>().changeGrounded(isGrounded); //updates the control gui

    }

    private void playerInput()
    {

        if (character.velocityHorizontal >= -0.1 && character.velocityHorizontal <= 0.1f) //If the player is moving update the currentVelocity
        {
            currentVelocity = character.speed;
        }


        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            state = State.STATE_ATTACKING;
            StartCoroutine("attack", 0.5f);
            character.animator.SetBool("Attacking", true);
            
        }

        if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown("q")) //pressing this button will show the last message displayed by a sign
        {
            levelManager.GetComponent<GuiObserver>().displayLastMessage();
        }


        //If the player blast cooldown timer is more than the limit then they will be able to shot a new blast
        if (character.blastCooldownTimer >= character.blastCooldownLimit)
        {
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown("joystick button 1"))
            {

                if (character.blastCooldownTimer < character.blastCooldownLimit)
                {
                    print("poof cooldown");
                }
                else
                {
                    character.blastCooldownStart = Time.time;
                    character.blastCooldownTimer = Time.time - character.blastCooldownStart;
                    shotBlast();
                }
            }
        }
        else
        {
            if (character.blastCooldownStart == 0)
            {
                character.blastCooldownStart = -character.blastCooldownLimit;
            }
            character.blastCooldownTimer = Time.time - character.blastCooldownStart;
        }

        levelManager.GetComponent<GuiObserver>().updateSphereCooldown(character.blastCooldownTimer / character.blastCooldownLimit);
 
        if (character.cooldownTimer >= character.cooldownLimit)
        {
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown("joystick button 2"))
            {
                if (character.cooldownTimer < character.cooldownLimit)
                {
                    print("Poof: cooldown");
                }
                else
                {
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

    private void shotMagic() //Handles everything when shooting a magic shot
    {
        //Spawn in the bullet prefab
        GameObject bullet = GameObject.Instantiate(magicBlastPrefab, new Vector2(transform.position.x + 0, transform.position.y), transform.rotation) as GameObject;
        //Get a random number and choose which sound that should be played
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 1)
        {
            GameManager.instance.playSoundEffect(8);
        }
        else
        {
            GameManager.instance.playSoundEffect(9);

        }
        //check if your facing left or right and shoot the bullet in that direction
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
        character.body.velocity = Vector3.zero;
        gameObject.transform.position = character.startingPosition;
    }

    private void checkRespawnHeight(int height)
    {
        if (transform.position.y < height)
        {
            character.body.velocity = Vector3.zero;
            GameManager.instance.playSoundEffect(1);
            transform.position = character.startingPosition;
        }
    }

    void movement()
    {
        character.velocityHorizontal = Input.GetAxisRaw("Horizontal");
        character.velocityVertical = Input.GetAxisRaw("Vertical");
        character.animator.SetFloat("speed", Mathf.Abs(character.velocityHorizontal));

        //If the player is moving right flip 
        if (character.velocityHorizontal > 0 && !isFacingRight)
            flip();
        else if (character.velocityHorizontal < 0 && isFacingRight)
            flip();

        if (isGrounded) //The player should only be able to jump is they are grounded
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

            character.body.velocity = new Vector2(character.body.velocity.x, character.jump);
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

    //Inverts the player sprite to look the other way
    private void flip()
    {
        
        isFacingRight = !isFacingRight;
        if (state.Equals(State.STATE_WALKING))
            currentVelocity = character.speed;

        Vector3 theScale = character.body.transform.localScale;
        theScale.x *= -1;
        character.body.transform.localScale = theScale;
    }

    //Checks for triggers entering the collision
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.tag.ToString().Equals("Ladder"))
        {
            state = State.STATE_CLIMBING;
            character.animator.SetBool("Climbing", true);
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
            character.animator.SetBool("Climbing", true);
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
            character.animator.SetBool("Climbing", false);
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
                        character.body.velocity = new Vector2(0, 0);
                        isGrounded = true;
                        state = State.STATE_WALKING;
                        character.animator.SetBool("Jumping", false);

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
                character.animator.SetBool("Jumping", false);
            }
            else if (coll.transform.tag.ToString().Equals("End"))
            {
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
    }
    

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!checkIfSide(coll))
        {
            if (coll.transform.tag.ToString().Equals("MovingPlatform"))
            {
                state = State.STATE_MOVINGPLATFORM;
                isGrounded = true;
                character.animator.SetBool("Jumping", false);
            }
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platforms") || coll.transform.tag.ToString().Equals("Platform") || coll.transform.tag.ToString().Equals("BreakingPlatform"))
            {
                if (checkIfTop(coll))
                {
                    isGrounded = true;
                    state = State.STATE_WALKING;
                    character.animator.SetBool("Jumping", false);
                }
                if (coll.contacts[0].normal == new Vector2(0, 1))
                {
                    character.animator.SetBool("Jumping", false);
                }
            }
            else if (coll.transform.tag.ToString().Equals("StairsPlatform"))
            {
                isGrounded = true;
            }
            else if (coll.transform.tag.ToString().Equals("Stairs"))  
            { 
                state = State.STATE_STAIRS;
            }
            else if (coll.transform.tag.ToString().Equals("Stairs_top"))
            {
                if (character.velocityVertical < -0.7 || character.velocityVertical > 0.7)
                {
                    coll.gameObject.GetComponent<Collider2D>().isTrigger = true;
                    state = State.STATE_STAIRS;
                    enableStairs(true);
                }
                else
                {
                    coll.gameObject.GetComponent<Collider2D>().isTrigger = false;
                    enableStairs(false);
                }
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
                isGrounded = false;
                character.animator.SetBool("Jumping", true);
            }
            else if (coll.transform.tag.ToString().Equals("Ground") || coll.transform.tag.ToString().Equals("Platforms") || coll.transform.tag.ToString().Equals("Platform"))
            {
                isGrounded = false;
                character.animator.SetBool("Jumping", true);
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
                }
            }
            else if (coll.transform.tag.ToString().Equals("Stairs"))
            {
                character.animator.SetBool("Climbing", false);
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
    }



    IEnumerator underWaterBreath()
    {
        yield return new WaitForSeconds(2);
        character.health = character.health - 25;
        hurt();
        StartCoroutine("underWaterBreath");
    }


    //When climbing stairs they are disabled, entering the topstairs triggers enables the stairs to allow you to climb them
    private void enableStairs(bool enable)
    {
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
        if (character.health <= 0)
        {
            state = State.STATE_DEATH;
        }

        levelManager.GetComponent<GuiObserver>().UpdateHealth(character.health);
    }

    //Used by the item to give the player health back
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

    //When this is called it plays the hurt sound, only when the health is more than 0
    private void hurt()
    {
        if (character.health > 0)
            GameManager.instance.playSoundEffect(0);
    }

}
