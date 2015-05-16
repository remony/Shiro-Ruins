using UnityEngine;
using System.Collections;

public class MovingPlatformController : MonoBehaviour
{

    Vector2 movement = Vector2.right;
    public float speed = 50f;
    bool hitWall = false;
    bool moveLeft = false;
    Rigidbody2D rigidbody;
    SliderJoint2D slider;

    

    // Use this for initialization
    void Start()
    {
        transform.FindChild("Collision").gameObject.tag = "MovingPlatform";
        gameObject.tag = "MovingPlatform";
        rigidbody = this.gameObject.AddComponent<Rigidbody2D>();
        slider = this.gameObject.AddComponent<SliderJoint2D>();
        rigidbody.mass = 150;
        gameObject.transform.position = new Vector2(transform.position.x - 20,transform.position.y);
        //gameObject.transform.position.x = gameObject.transform.position.x - 20;
        rigidbody.gravityScale = 150;
        rigidbody.isKinematic = false;
        rigidbody.fixedAngle = true;
    }

    // Update is called once per frame
    void Update()
    {

        slider.connectedAnchor = new Vector2(transform.position.x, transform.position.y);
        slider.enableCollision = true;
        slider.angle = 0;


        Vector2 movement = this.gameObject.GetComponent<Rigidbody2D>().velocity;

        if (moveLeft)
            movement = new Vector2(1 * -speed, 0);
        else if (!moveLeft)
            movement = new Vector2(1 * speed, 0);




        this.gameObject.GetComponent<Rigidbody2D>().velocity = movement;



        //Debug.Log("moving");
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
       // Debug.Log("Platform hit " + coll.collider.tag);
        if (coll.collider.tag == "Ground" || coll.collider.tag == "Platform")
            moveLeft = !moveLeft;

        if (coll.collider.tag == "Player")
        {
            GameObject.Find("Player").transform.parent = this.gameObject.transform;
            rigidbody.isKinematic = false;
        }
    }


    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameObject.Find("Player").transform.parent = null;

        }
    }
    


}

