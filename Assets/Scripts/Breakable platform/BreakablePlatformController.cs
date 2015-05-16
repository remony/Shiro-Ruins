using UnityEngine;
using System.Collections;

public class BreakablePlatformController : MonoBehaviour {
    BreakablePlatform breakablePlatform;

    public float timeToLive = 2;
    public float timeToRecover = 5;
    public bool playerIsHere = false;
    public bool recovering = false;
    public Sprite platform;
    public Sprite broken_platform;

	// Use this for initialization
	void Start () {
        breakablePlatform = new BreakablePlatform();
        breakablePlatform.timeToLive = timeToLive;
        breakablePlatform.timeToRecover = timeToRecover;
        breakablePlatform.spawn = gameObject.transform;
	}

    //The time until the plarform breaks
    IEnumerator timeToBreak()
    {
        yield return new WaitForSeconds(breakablePlatform.timeToLive);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = broken_platform;
        StartCoroutine("timeToRevive");
        
    }

    //The time until the platform revives appears again
    IEnumerator timeToRevive()
    {
        recovering = true;
        yield return new WaitForSeconds(breakablePlatform.timeToRecover);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = platform;
        recovering = false;
    }

    
    void OnCollisionEnter2D(Collision2D coll)
    {
        //When the player enters a collision with the platofmr start the break timer
        if (coll.transform.tag.ToString().Equals("Player"))
        {
            playerIsHere = true;
            StartCoroutine("timeToBreak");
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        //WHen the player stays on the platform set the playerishere boolean
        if (coll.transform.tag.ToString().Equals("Player"))
        {
            //only set when it is false...
            if (!playerIsHere)
                playerIsHere = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        //if the player has left the platform set it to false
        if (coll.transform.tag.ToString().Equals("Player"))
        {
            playerIsHere = false;
        }
    }
}
