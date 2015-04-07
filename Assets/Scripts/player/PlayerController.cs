using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {



	public float speed = 2.50f;
	public float jumpHeight = 200f;
	private Vector2 movement = new Vector2(1, 1);
	public GameObject Character;
	public bool grounded = false;

	public AudioClip shootSound;
    public AudioClip AttackSound;



	public int jumpLimit = 2;
	private int jumpCount = 2;
	private Animator animator;
	private AudioSource source;
    private int animationState = 0;



    //Controller setup
    public Vector3 controller_movement;
    private float movementSpeed = 8;
    private float jump = 15;
    private float gravity = 40;





	void Awake()
	{
        source = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
		animator = Character.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

        controller_movement.x = Input.GetAxis("LeftJoystickX") * movementSpeed;
        controller_movement.y = Input.GetAxis("LeftJoystickY") * movementSpeed;

        if(grounded)
        {
            if(Input.GetButtonDown("A"))
            {
                controller_movement.y = jump;
            }
        }

        //controller_movement.y -=  * Time.deltaTime;
        transform.Translate(Vector3.left * Time.deltaTime * gravity, Space.World);



        if (grounded) {
			/*
         *     Player Movement Jump      
         * 
         */
			
			if (Input.GetKeyDown(KeyCode.Space) )
			{
                /*
				//rigidbody2D.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
				if (jumpCount <= (jumpLimit - 1))
				{
					jumpCount++;
					//rigidbody2D.AddForce(new Vector2(0f, 500));

        */
                source.PlayOneShot(shootSound,1f);
                    animator.SetTrigger("Jumping");

                    GetComponent<Rigidbody>().AddForce(new Vector2(0f, jumpHeight));
                //rigidbody2D.AddForce(Vector2.up * jumpHeight);
                //transform.Translate(Vector3.up * 20 * (Time.deltaTime * speed), Space.World);
                /*		

                    }
                    else
                    {
                        Debug.Log("Jump limit reached");
                    }

                    Debug.Log(jumpCount);

                     */
            }

        }




        if (Input.GetKeyDown(KeyCode.Mouse1))   {
            animator.SetInteger("shiro", 4);
            source.PlayOneShot(AttackSound, 1f);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetInteger("shiro", 0);
        }



        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {

           animator.SetBool("Walking", true);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);

                Vector3 theScale = transform.localScale;
                if (theScale.x != 1)
                {
                    theScale.x *= -1;
                }
                transform.localScale = theScale;

               
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);

                Vector3 theScale = transform.localScale;
                if (theScale.x != -1)
                {
                    theScale.x *= -1;
                }

                transform.localScale = theScale;

            }


        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (grounded)
            {
                animator.SetBool("Walking", false);
            }
        }








    }

	void OnCollisionStay(Collision coll)
	{
		if (coll.gameObject.layer == 8 || coll.gameObject.layer == 9)
		{
			
			//If on ground and jump count has not restarted, reset
			if (jumpCount == jumpLimit)
				jumpCount = 0;
			
			grounded = true;
		}
		
		
		
	}

	void OnCollisionEnter(Collision coll)
	{
		//Resets the jump count as the player touches the ground
		jumpCount = 0;
        Debug.Log("yay");

        if (animationState == 3)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                animator.SetInteger("shiro", 3);
            }
            else
            {
                animator.SetInteger("shiro", 0);
            }
        }   else
        {
            animator.SetInteger("shiro", 0);
        }

    }
        
	
	void OnCollisionExit(Collision coll)
	{
		grounded = false;
	}

    /*
    void Update()
    {
        if (grounded)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Debug.Log(animationState);
                if (animationState != 3)
                {
                    Debug.Log("walking");
                    animationState = 3;
                    animator.SetInteger("shiro", 3);
                }
                else
                {
                    Debug.Log("Already Walking");
                }

            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                animationState = 0;
                animator.SetInteger("shiro", 0);
            }
        }
    }
       
    */ 

}
