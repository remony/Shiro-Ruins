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


    private Rigidbody2D body;

    float distFromTarget = 1;

	// Use this for initialization
	void Start () {
        base.Start();
        enemy = new Enemy();
        enemy.health = 100;
        enemy.jump = 350;
        enemy.speed = 50;
        enemy.attackPower = 15;
        
        print("Enemy health: " + enemy.health);

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


            print("there is " + players.Length + " PLayers");
        
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
        


        //print(gameObject.transform.position.x - target.position.x);
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
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag.ToString().Equals("MagicBullet"))
        {
            print("ow bullet");
                enemy.health -= 10;
            
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
        
    }

    IEnumerator Attack(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);
        strike(player);
        
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
