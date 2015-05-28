using UnityEngine;
using System.Collections;

public class ItemController : ItemStateHandler
{
    public Item item;

    public override void onIdle()
    {

    }
    public override void onTaken()
    {

    }
	// Use this for initialization
    new
	void Start () {
        base.Start();
        item = new Item();
        if (item.value == 0)
        {
            item.value = 100;
        }
        item.body = gameObject.AddComponent<Rigidbody2D>();
        item.boxCollider = gameObject.AddComponent<BoxCollider2D>();
        item.body.isKinematic = false;
        item.body.fixedAngle = true;
        item.body.gravityScale = 0;
        item.boxCollider.size = new Vector2(0.2f, 0.2f);
        item.boxCollider.isTrigger = true;
        item.levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        item.audioSource = gameObject.GetComponent<AudioSource>();
        
        int ran = Random.Range(0, 2);
        //set a random sound
        switch (ran)
        {
            case 0:
                item.pickupSound = Resources.Load("sounds/pickup") as AudioClip;
                break;
            case 1:
                item.pickupSound = Resources.Load("sounds/pickup2") as AudioClip;
                break;
        }
	}
	
	// Update is called once per frame
    new
	void Update () {
        base.Update();
	}

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            item.audioSource.PlayOneShot(item.pickupSound, 1f);
            int willHeal = Random.Range(0,2);
            if (item.levelManager != null)
            {
                if (willHeal == 1)
                {
                    int health = Random.Range(10, 25);
                    coll.GetComponent<CharacterController>().heal(health);
                }
                item.levelManager.GetComponent<GuiObserver>().AddScore(item.value);
            }   
            StartCoroutine("destroy");
        }

    }


    //Allows time for sound to play before the item breaks
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.13f);
        Destroy(gameObject);

    }
}

