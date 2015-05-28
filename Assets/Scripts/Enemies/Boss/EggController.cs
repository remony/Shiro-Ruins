using UnityEngine;
using System.Collections;

public class EggController : MonoBehaviour {


    public bool isRight = true;
    public GameObject explosion;
    public Egg eggBullet;
    private Rigidbody2D body;
    public bool collided = false;
    public int speed = 400;
    private GameObject target;
    private Vector2 targetPosition;



    // Use this for initialization
    void Start()
    {
        eggBullet = new Egg();
        eggBullet.speed = speed;
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        //print(eggBullet.speed);

        if (!isRight)
        {
            eggBullet.speed = -eggBullet.speed;
        }

        body = GetComponent<Rigidbody2D>();
        if (collided)
        {
            body.velocity = Vector3.zero;
        }
        else
        {
            body.velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * speed;
        }

        StartCoroutine("timeToLive");
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
        

        if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
        {
            //body.velocity =(transform.position.x + speed, transform.position.y) * speed;
            body.velocity = new Vector2(body.velocity.x * speed, body.velocity.y);
            //Destroy(gameObject);
        }
        else
        {
            if (collided)
            {
                body.velocity = Vector3.zero;

            }
            else
            {
                //body.velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * speed;
                body.velocity = new Vector2(body.velocity.x, body.velocity.y);
                //transform.position = Vector2.MoveTowards(transform.position, targetPosition, (speed * Time.deltaTime));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            gameObject.GetComponent<Animator>().SetBool("Explode", true);
            collided = true;
            changeSize();

            coll.transform.GetComponent<CharacterController>().character.health -= 5;
            Rigidbody2D body = coll.transform.GetComponent<Rigidbody2D>();
            if (coll.gameObject.GetComponent<CharacterController>().isFacingRight)
            {
                //body.velocity = new Vector2(coll.gameObject.GetComponent<CharacterController>().currentVelocity * body.velocity.x + 400, body.velocity.y + 10);
                body.AddForce(new Vector2(1200, 405), ForceMode2D.Impulse);
            }
            else
            {
                //body.velocity = new Vector2(coll.gameObject.GetComponent<CharacterController>().currentVelocity * body.velocity.x + -4100, body.velocity.y + 10);
                body.AddForce(new Vector2(-1200, 405), ForceMode2D.Impulse);
            }
            StopCoroutine("timeToLive");
            StartCoroutine("collision");
            //Destroy(gameObject);
        }
        else if (coll.tag.ToString().Equals("Ground") || coll.tag.ToString().Equals("Platform"))
        {
            collided = true;
            gameObject.GetComponent<Animator>().SetBool("Explode", true);
            StartCoroutine("collision");
        }
        else if (coll.tag.Equals("MagicBullet") || coll.tag.Equals("MagicBlast"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            transform.position = coll.transform.position;
        }

    }

    IEnumerator collision()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }


    IEnumerator timeToLive()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }


    private void changeSize()
    {
        gameObject.transform.localScale = new Vector2(128f, 128f);
    }
}
