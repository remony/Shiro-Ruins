using UnityEngine;
using System.Collections;

public class EnemyController : EnemyStateHandler {

    public Enemy enemy;
    //private GameObject gameObject;
    private Animator animator;
    public GameObject target;

    public bool movingRight = true;
    public float distance = 100;
    private Transform startingPos;
    private float endingPos;

    private float velocity;

    public bool grounded;

    private Rigidbody2D body;

    float distFromTarget = 1;

    void createCharacter()
    {
        enemy = new Enemy();
        enemy.health = 100;
        enemy.jump = 350;
        enemy.speed = 50;
        enemy.attackPower = 15;
    }

	// Use this for initialization
	void Start () {
        base.Start();
        createCharacter();
        print("hwllo");
        //StartCoroutine("hop");
        //print("Enemy health: " + enemy.health);

        startingPos = gameObject.transform;
        endingPos = startingPos.position.x + distance;
        //endingPos = gameObject.transform;
        //endingPos.position = new Vector2(startingPos.position.x + distance, startingPos.position.y);

        //print("Enemy distance: starting: " + startingPos + " ending: " + endingPos + " | total distance apart: " + (endingPos - startingPos));
        try
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        catch (System.Exception e)
        {
            Debug.Log("no player");
        }
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //GameObject oh = new GameObject();
        //for (int i = 0; i < players.Length; i++ )
        //{
         //   print(i);
          //  players[i].transform.parent = oh.transform;
        //}


           // print("there is " + players.Length + " PLayers");
        
        animator = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        body.fixedAngle = true;
        flip();

        velocity = enemy.health;
        animator.SetFloat("Velocity", Mathf.Abs(velocity));
	}


    public override void onIdle()
    {


        velocity = 0;
        
        ////move towards the player
        //if (Vector3.Distance(transform.position, target.position) > 1f)
        //{//move if distance from target is greater than 1
            state = State.STATE_WALKING;
        //}
    }

    public override void onWalking()
    {
        if (movingRight)
        {
            distFromTarget = gameObject.transform.position.x - endingPos;
            //print(gameObject.transform.position.x + " + " + endingPos + " = " + distFromTarget);
            float step = enemy.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(endingPos , gameObject.transform.position.y), step);



            //print("moving right");
        }   
        
        if (!movingRight)
        {
            distFromTarget = (endingPos - distance) - transform.position.x;
            //print(transform.position.x + " + " + (startingPos.position.x + distance) + " = " + distFromTarget);
            float step = enemy.speed * Time.deltaTime;

            //print(startingPos.position.x);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(endingPos - distance, gameObject.transform.position.y), step);
            

            //print("moving left");
        }

        //print(movingRight + "  |  " + distFromTarget);
        if (distFromTarget == 0)
        {
           movingRight = !movingRight;
           flip();
       }

      
        
        
    }
    
    public override void onFollow()
    {
        float step = enemy.speed * Time.deltaTime;
        if (target.transform.position.y > transform.position.y)
        {
            if (Random.Range(0, 40) == 20)
            {
                jump();
            }
        }
        
        //print(startingPos.position.x);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, gameObject.transform.position.y), step);
    }

    public override void onAttacking()
    {
        velocity = enemy.speed;
        float step = enemy.speed * Time.deltaTime;

        //print(startingPos.position.x);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x + 1, gameObject.transform.position.y + 100), step);
        
    }

    private void fallback()
    {
        float step = enemy.speed * Time.deltaTime;
        if (movingRight)
        {
            body.AddForce(new Vector2(-400, -405), ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(new Vector2(400, 405), ForceMode2D.Impulse);
        }
        
        state = State.STATE_IDLE;
    }


	
	// Update is called once per frame
	void Update () {
        base.Update();


        if (enemy.health == 0 || enemy.health < 0)
        {
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GuiObserver>().AddScore(250);
            Destroy(gameObject);
        }
        //  print(Vector2.Distance(gameObject.transform.position, target.transform.position));
        if (target != null)
        {
            if (grounded)
            {
                float distance = Vector2.Distance(gameObject.transform.position, new Vector2(target.transform.position.x, target.transform.position.y - 20));
                //print(distance);
                if (distance < 200 && distance > 35)
                {
                    state = State.STATE_FOLLOWING;

                }
                else if (distance > 0 && distance < 35 || distance == 0)
                {
                    state = State.STATE_ATTACKING;
                }
                else
                {
                    state = State.STATE_WALKING;
                }
            }


        }

 
        


        //print(gameObject.transform.position.x - target.position.x);
	}


    public bool checkCollision(Collision2D coll)
    {

        //pointOfContact = colliderWeTouched.contacts[0].normal; //Grab the normal of the contact point we touched
        Debug.Log(coll.contacts.Length);
        Debug.Log(coll.contacts[0].point);

        


        //Detect which side of the collider we touched
        if (coll.contacts[0].normal == new Vector2(-1, 0))
        {
            Debug.Log("left side!");
            return true;
        }

        if (coll.contacts[0].normal == new Vector2(1, 0))
        {
            Debug.Log("We touched the right side of the enemy!");
            return true;
        }
        /*
        if (coll.contacts[0].normal == new Vector2(0, -1))
        {
            Debug.Log("We touched the enemy's bottom!");
        }

        if (coll.contacts[0].normal == new Vector2(0, 1))
        {
            Debug.Log("We touched the top of the enemy!");
            
        }
         * */
        return false;
    }

    void jump()
    {
        
        body.AddForce(new Vector2(0, 470), ForceMode2D.Impulse);
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag.ToString().Equals("Player"))
        {
            if (state.Equals(State.STATE_ATTACKING))
            {
                state = State.STATE_ATTACKING;
                StartCoroutine("Attack", coll.gameObject);
            }
            
        }   
        else if (coll.transform.tag.ToString().Equals("Platform"))
        {
            if (checkCollision(coll))
            {
                jump();

            }
            grounded = true;
            //bool waiting = false;
            

        }
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag.ToString().Equals("MagicBullet"))
        {
            print("ow bullet");
                enemy.health -= 10;
            
        }
        else if (coll.transform.tag.ToString().Equals("Spike"))
        {
            enemy.health = 0;
        }
    }


    void OnCollisionStay2D(Collision2D coll)
    {


        if (coll.transform.tag.ToString().Equals("Player"))
        {
            if (coll.gameObject.GetComponent<CharacterController>().state.ToString().Equals("STATE_ATTACKING"))
            {
                enemy.health -= 10;
            }
        }
        else if (coll.transform.tag.ToString().Equals("Platform"))
        {
            grounded = true;
            
            
            //movingRight = !movingRight;
            //body.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        }
        
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag.ToString().Equals("Ground"))
        {
            grounded = false;
        }
    }

    IEnumerator Attack(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);
        strike(player);
        
    }

    IEnumerator hop()
    {
        yield return new WaitForSeconds(0.4f);
        body.AddForce(new Vector2(50, 100), ForceMode2D.Impulse);
        //StartCoroutine("hop");
    }

    IEnumerator airJump()
    {
        yield return new WaitForSeconds(1);
        if (!grounded)
            movingRight = !movingRight;
    }

    private void strike(GameObject player)
    {
        if (state.Equals(State.STATE_ATTACKING))
        {
            player.GetComponent<CharacterController>().character.health -= enemy.attackPower;
            print("STRIKE");
            fallback();
        }
        
    }

    private void flip()
    {

        // Multiply the player's x local scale by -1.
        Vector3 theScale = body.transform.localScale;
        theScale.x *= -1;
        body.transform.localScale = theScale;
    }
}
