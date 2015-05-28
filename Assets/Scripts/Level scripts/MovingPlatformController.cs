using UnityEngine;
using System.Collections;

public class MovingPlatformController : MonoBehaviour
{


    public float speed = 50f;
    bool moveLeft = false;
    Rigidbody2D body;
    SliderJoint2D slider;

    

    // Use this for initialization
    void Start()
    {
        transform.FindChild("Collision").gameObject.tag = "MovingPlatform";
        gameObject.tag = "MovingPlatform";
        gameObject.AddComponent<Rigidbody2D>();
        body = gameObject.GetComponent<Rigidbody2D>();
        slider = gameObject.AddComponent<SliderJoint2D>();
        //slider.rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        body.mass = 150;
        gameObject.transform.position = new Vector2(transform.position.x - 20,transform.position.y);
        //gameObject.transform.position.x = gameObject.transform.position.x - 20;
        body.gravityScale = 150;
        body.isKinematic = false;
        body.fixedAngle = true;
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
        if (coll.collider.tag == "Ground" || coll.collider.tag == "Platforms" || coll.collider.tag.Equals("Platform"))
            moveLeft = !moveLeft;

        if (coll.collider.tag == "Player")
        {
            GameObject.Find("Player").transform.parent = this.gameObject.transform;
            body.isKinematic = false;
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

