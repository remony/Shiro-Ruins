using UnityEngine;
using System.Collections;
using System;

public class ConsoleController : MonoBehaviour {

    private bool isFacingRight;
    private bool onWall;
    private bool onLadder = false;
    private bool inWater = false;


    public float speed = 210f;
    public GameObject spritesheet;
    private Animator animator;
    private bool isGrounded = false;
    private bool onMovingPlatform = false;
    private GameObject character;
    private float velocity = 0f;
    private float lift = 0f;

    RaycastHit2D hit;
    public Transform sightStart, sightEnd;

    Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        character = this.gameObject;
        animator = spritesheet.GetComponent<Animator>();
        rigidbody = character.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 150;
        rigidbody.mass = 0;
        rigidbody.isKinematic = false;
        rigidbody.fixedAngle = true;
	}
	
	// Update is called once per frame
	void Update () {
        velocity = Input.GetAxisRaw("Horizontal");
        lift = Input.GetAxisRaw("Vertical");
        animator.SetFloat("speed", Mathf.Abs(velocity));
     

        /*RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - start), new Vector2(transform.position.x - 1, transform.position.y - finish), 1);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - start), new Vector3(transform.position.x, transform.position.y - finish), Color.red);



        RaycastHit2D wallRight = Physics2D.Raycast(new Vector2(transform.position.x + 10, transform.position.y + 5), new Vector2(transform.position.x, transform.position.y), 1);
        Debug.DrawLine(new Vector3(transform.position.x + 10, transform.position.y), new Vector3(transform.position.x+5, transform.position.y), Color.red);


        RaycastHit2D wallLeft = Physics2D.Raycast(new Vector2(transform.position.x - 10, transform.position.y), new Vector2(transform.position.x -5, transform.position.y), 1);
        Debug.DrawLine(new Vector3(transform.position.x - 10, transform.position.y), new Vector3(transform.position.x -5, transform.position.y), Color.red);


        print("on wall " + onWall);

        if (wallLeft.collider != null)
        {
            if (!isGrounded)
            {
                //onWall = true;
                print("on wall");
            }
                
        }
        else
        {
            //onWall = false;
        }
        
        if (hit.collider != null)
        {
            isGrounded = true;
            print(hit.collider.tag);
        }
        else
        {
            isGrounded = false;
            print("not on ground");
            //print(hit.collider.tag);
        }
         * */
 

        
         
        if (velocity > 0 && !isFacingRight)
            flip();
        if (velocity < 0 && isFacingRight)
            flip();


        
            if (onMovingPlatform)
            {
                if (velocity == 0)
                {
                    rigidbody.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, rigidbody.velocity.y);
                }
                else
                {
                    rigidbody.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((speed / transform.parent.GetComponent<Rigidbody2D>().velocity.x) * velocity), rigidbody.velocity.y);
                }

                //Debug.Log(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
            }
            else if (!onMovingPlatform)
            {
                if (onLadder)
                {
                    rigidbody.velocity = new Vector2(velocity * speed, lift * speed);
                }
                else if (inWater)
                {
                    rigidbody.velocity = new Vector2(((velocity * speed) * 0.75f), rigidbody.velocity.y);
                }
                else
                {
                    rigidbody.velocity = new Vector2(velocity * speed, rigidbody.velocity.y);
                }

                
                //Debug.Log(velocity);
            }


            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
            {
                if (isGrounded)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.y, 450);
                }


            }

        
  
    }


    private void flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = rigidbody.transform.localScale;
        theScale.x *= -1;
        rigidbody.transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag.ToString().Equals("MovingPlatform"))
        {
            onMovingPlatform = true;
            isGrounded = true;
            rigidbody.gravityScale = 0;
            rigidbody.mass = 0;
            rigidbody.isKinematic = false;
            animator.SetBool("Jumping", false);
        }
        if (coll.collider.tag.ToString().Equals("Ground"))
        {

                isGrounded = true;
                animator.SetBool("Jumping", false);
            


            
        }
    }


    void OnCollisionStay2D(Collision2D coll)
    {
        //Debug.Log("Hello " + coll.collider.tag);
        if (coll.collider.tag.ToString().Equals("Ground"))
        {
            if (coll.transform.position.y > 0)
            {
                print("hey yor on top");
                isGrounded = true;
                animator.SetBool("Jumping", false);
            }
        }


        if (coll.collider.tag.ToString().Equals("MovingPlatform"))
        {
            onMovingPlatform = true;
            isGrounded = true;
            rigidbody.gravityScale = 0;
            rigidbody.mass = 0;
            rigidbody.isKinematic = false;
            animator.SetBool("Jumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag.ToString().Equals("Ground"))
        {
            isGrounded = false;
            animator.SetBool("Jumping", true);
        }
        
        if (coll.collider.tag.ToString().Equals("MovingPlatform"))
        {
            onMovingPlatform = false;
            isGrounded = false;
            rigidbody.gravityScale = 150;
            rigidbody.mass = 0;
            rigidbody.isKinematic = false;
            rigidbody.fixedAngle = true;
            animator.SetBool("Jumping", true);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Untagged"))
        {
            Debug.Log("Trigger hit " + coll.GetComponent<Collider2D>().tag);
            rigidbody.gravityScale = 0;
            onLadder = true;
            Debug.Log("Hit trigger");
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            rigidbody.gravityScale = 75;
            inWater = true;
        }
        
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Untagged"))
        {
            Debug.Log("Trigger hit " + coll.GetComponent<Collider2D>().tag);
            onLadder = false;
            rigidbody.gravityScale = 150;
            Debug.Log("Hit trigger");
        }
        else if (coll.tag.ToString().Equals("Water"))
        {
            rigidbody.gravityScale = 150;
            inWater = true;
        }
    }
    
}
