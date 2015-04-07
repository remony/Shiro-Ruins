using UnityEngine;
using System.Collections;
using System;

public class ConsoleController : MonoBehaviour {

    private bool isFacingRight;
    public float speed = 210f;
    public GameObject spritesheet;
    private Animator animator;
    private bool isGrounded = false;
    private bool onMovingPlatform = false;
    private GameObject character;
    private float velocity = 0f;

    Rigidbody2D rigidbody = null;

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
        animator.SetFloat("speed", Mathf.Abs(velocity));

        
        

        if (velocity > 0 && !isFacingRight)
            flip();
        if (velocity < 0 && isFacingRight)
            flip();


       
        if (onMovingPlatform)
        {
            if (velocity == 0)
            {
                rigidbody.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, rigidbody.velocity.y);
            }   else 
            {
                rigidbody.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x * ((speed / transform.parent.GetComponent<Rigidbody2D>().velocity.x)* velocity), rigidbody.velocity.y);
            }

            //Debug.Log(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
        }
        else if (!onMovingPlatform)
        {
            rigidbody.velocity = new Vector2(velocity * speed, rigidbody.velocity.y);
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
        Debug.Log("Hello " + coll.collider.tag);
        if (coll.collider.tag.ToString().Equals("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jumping", false);
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
        Debug.Log("Trigger hit " + coll.GetComponent<Collider>().tag);
        Debug.Log("Hit trigger");
    }
    
}
