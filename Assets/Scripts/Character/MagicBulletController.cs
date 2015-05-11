using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicBulletController : MonoBehaviour {

    
    public bool isRight = true;
    public GameObject explosion;
    public MagicBullet magicBullet;
    




	// Use this for initialization
	void Start () {
        magicBullet = new MagicBullet();
        magicBullet.speed = 200;
        print(magicBullet.speed);

        if (!isRight)
        {
            magicBullet.speed = -magicBullet.speed;
        }

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.velocity = new Vector2(magicBullet.speed, body.velocity.y);
        StartCoroutine("timeToLive");
        gameObject.GetComponent<Collider2D>().isTrigger = true;
	}

	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Enemy"))
        {

            explosion.SetActive(true);
            StartCoroutine("collision");
            //Destroy(gameObject);
        }
}
    IEnumerator collision()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


    IEnumerator timeToLive()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


    

}

class MagicBulletDestroy: MonoBehaviour
{
    void OnEnable()
    {
        Invoke("Destroy", 2f);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
