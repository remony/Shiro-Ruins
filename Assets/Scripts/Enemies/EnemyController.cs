using UnityEngine;
using System.Collections;

public class EnemyController : EnemyStateHandler {

    public Enemy enemy;
    
    public GameObject target;
    public bool movingRight = true;
    public float distance = 100;


    private Animator animator;
    private Transform startingPos;
    private float endingPos;
    private float velocity;
    private bool grounded;
    private Rigidbody2D body;
    private float distFromTarget = 1;

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
        startingPos = gameObject.transform;
        endingPos = startingPos.position.x + distance;
        //find the player, there should always be one but just incase
        try
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        catch (System.Exception e)
        {
            Debug.Log("No Player has been detected..." + e);
        }
        

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
        state = State.STATE_WALKING;
    }

    public override void onWalking()
    {
        



        if (movingRight)
        {
            distFromTarget = gameObject.transform.position.x - endingPos;
            float step = enemy.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(endingPos , gameObject.transform.position.y), step);
        }   
        
        if (!movingRight)
        {
            distFromTarget = (endingPos - distance) - transform.position.x;
            float step = enemy.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(endingPos - distance, gameObject.transform.position.y), step);
        }

        if (distFromTarget == 0)
        {
            movingRight = !movingRight;
            flip();
        }  
    }
    
    public override void onFollow()
    {
        float step = enemy.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, gameObject.transform.position.y), step);
        if (target.transform.position.x > transform.position.x)
        {
            movingRight = false;
        }
        else if (target.transform.position.x < transform.position.x)
        {
            movingRight = true;
        }
    }

    public override void onAttacking()
    {
        velocity = enemy.speed;
        float step = enemy.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x + 1, gameObject.transform.position.y + 100), step);
        
    }

    //Fallback forces the object to be pushed backwards, this is more of a behaviour. 
    private void fallback()
    {
        float step = enemy.speed * Time.deltaTime;
        if (movingRight)
        {
            body.AddForce(new Vector2(200, 405), ForceMode2D.Impulse);
        }
        else if (!movingRight)
        {
            body.AddForce(new Vector2(-200, 405), ForceMode2D.Impulse);
        }
        
        state = State.STATE_IDLE;
    }

	// Update is called once per frame
    void Update()
    {
        base.Update();
        if (enemy.health == 0 || enemy.health < 0)
        {
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GuiObserver>().AddScore(250);
            Destroy(gameObject);
        }

        if (target != null)
        {

            float distance = Vector2.Distance(gameObject.transform.position, new Vector2(target.transform.position.x, target.transform.position.y - 20));

            if (distance < 200 && distance > 35)
            {
                state = State.STATE_FOLLOWING;

            }
            else if (distance > 0 && distance < 60 || distance == 0)
            {
                state = State.STATE_ATTACKING;
            }
            else
            {
                
                state = State.STATE_WALKING;
            }

        }
    }


    public bool checkCollision(Collision2D coll)
    {
         //Detect which side of the collider we touched
        if (coll.contacts[0].normal == new Vector2(-1, 0)) //left
        {
            return true;
        }

        if (coll.contacts[0].normal == new Vector2(1, 0)) //right
        {
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
                //strike(coll.gameObject);
                StartCoroutine("Attack", coll.gameObject);
            }
            if (coll.contacts[0].normal == new Vector2(0, -1))
            {
                if (coll.gameObject.GetComponent<CharacterController>().isFacingRight)
                {
                    coll.gameObject.GetComponent<CharacterController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 6405), ForceMode2D.Impulse);
                }
                else
                {
                    coll.gameObject.GetComponent<CharacterController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(-200, 6405), ForceMode2D.Impulse);
                }
               // coll.gameObject.GetComponent<CharacterController>().GetComponent<Rigidbody2D>()
                enemy.health = 0;
            }
           
        }   
        else if (coll.transform.tag.ToString().Equals("Platform"))
        {
            if (checkCollision(coll))
            {
                jump();

            }
            grounded = true;
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag.ToString().Equals("MagicBullet"))
        {
            enemy.health -= (100 / 3);
        }
        else if (coll.transform.tag.ToString().Equals("Spike"))
        {
            enemy.health = 0;
        }
        else if (coll.transform.tag.ToString().Equals("Lava"))
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
    }

    private void strike(GameObject player)
    {
        if (state.Equals(State.STATE_ATTACKING))
        {
            fallback();
            player.GetComponent<CharacterController>().character.health -= enemy.attackPower;
            
        }
    }

    private void flip()
    {
        Vector3 theScale = body.transform.localScale;
        theScale.x *= -1;
        body.transform.localScale = theScale;
    }
}
