using UnityEngine;
using System.Collections;


public class Item2Controller : ItemStateHandler
{
    public Item item;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private AudioClip pickupSound;
    private AudioSource audioSource;
    public GameObject levelManager;

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
            item.value = 200;
        }
        body = gameObject.AddComponent<Rigidbody2D>();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        body.isKinematic = false;
        body.fixedAngle = true;
        body.gravityScale = 0;

        boxCollider.size = new Vector2(0.1f, 0.1f);
        boxCollider.isTrigger = true;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        audioSource = gameObject.AddComponent<AudioSource>();

        int ran = Random.Range(0, 1);
        switch (ran)
        {
            case 0:
                pickupSound = Resources.Load("sounds/pickup") as AudioClip;
                break;
            case 1:
                pickupSound = Resources.Load("sounds/pickup2") as AudioClip;
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
            
            audioSource.PlayOneShot(pickupSound, 1f);
            levelManager.SendMessage("AddScore", item.value);
            StartCoroutine("destroy");
        }

    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.14f);
        Destroy(gameObject);

    }
}
