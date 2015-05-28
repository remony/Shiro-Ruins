using UnityEngine;
using System.Collections;

public class MagicBlastController : MonoBehaviour {

    public bool isRight = true;
    public GameObject player;

	// Use this for initialization
	void Start () {
        StartCoroutine("timeToLive");
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (player)
        {
            transform.position = player.gameObject.transform.position;
        }
        else if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Enemy"))
        {
            if (coll.gameObject.GetComponent<EnemyController>())
            {
                if (coll.gameObject.GetComponent<EnemyController>().movingRight)
                {
                    coll.gameObject.GetComponent<EnemyController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 405), ForceMode2D.Impulse);
                }
                else
                {
                    coll.gameObject.GetComponent<EnemyController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(-200, 405), ForceMode2D.Impulse);
                }
            }

            if (coll.gameObject.GetComponent<HenController>())
            {
                if (coll.gameObject.GetComponent<HenController>().hen.facingRight)
                {
                    coll.gameObject.GetComponent<HenController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 405), ForceMode2D.Impulse);
                }
                else
                {
                    coll.gameObject.GetComponent<HenController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(-200, 405), ForceMode2D.Impulse);
                }
            }
            
            GameManager.instance.playSoundEffect(1);
            
        }

    }

    IEnumerator timeToLive()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
