using UnityEngine;
using System.Collections;

public class EnemyController : EnemyStateHandler {

    public Enemy enemy;
    //private GameObject gameObject;
    private Animator animator;
    public Transform target;

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
        print("Enemy health: " + enemy.health);

        startingPos = gameObject.transform;
        endingPos = startingPos.position.x + distance;
        //endingPos = gameObject.transform;
        //endingPos.position = new Vector2(startingPos.position.x + distance, startingPos.position.y);

        //print("Enemy distance: starting: " + startingPos + " ending: " + endingPos + " | total distance apart: " + (endingPos - startingPos));

        animator = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        body.fixedAngle = true;
        flip();
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
        velocity = enemy.speed;

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
     
        


        //transform.Translate(new Vector3(-enemy.speed * Time.deltaTime, 0, 0));
/*
        if (distFromTarget < 120 && distFromTarget > 20)
        {
            if (distFromTarget > 0)
            {
                transform.position += -transform.right * enemy.speed * Time.deltaTime;
            }
            else if (distFromTarget < -0)
            {
                transform.position += transform.right * -enemy.speed * Time.deltaTime;
            }
        }
        else if (distFromTarget < 10 && distFromTarget > -10)
        {
            state = State.STATE_ATTACKING;
        } 
*/
      
        
        
    }

    public override void onJumping()
    {

    }

    public override void onAttacking()
    {
        if (distFromTarget > 10 || distFromTarget < -10)
        {
            state = State.STATE_WALKING;
        }
    }


	
	// Update is called once per frame
	void Update () {
        base.Update();
        //print(endingPos.position.x - gameObject.transform.position.x);
        if (enemy.health == 0)
        {
            Destroy(gameObject);
        }

        animator.SetFloat("Velocity", Mathf.Abs(velocity));


        //print(gameObject.transform.position.x - target.position.x);
	}

    private void flip()
    {

        // Multiply the player's x local scale by -1.
        Vector3 theScale = body.transform.localScale;
        theScale.x *= -1;
        body.transform.localScale = theScale;
    }
}
