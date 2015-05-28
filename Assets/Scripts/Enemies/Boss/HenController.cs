using UnityEngine;
using System.Collections;

public class HenController : HenStateHandler
{
    public GameObject LevelScriptController;
    public Hen hen;
    private GameObject guiManager;
    public bool hardcoreBoss = false;
    public GameObject EggPrefab;

	// Use this for initialization
    new
	void Start () {
        base.Start();
        hen = new Hen();
        hen.size = 512;
        hen.health = 1000;
        hen.attackPower = 10;
        hen.animator = GetComponent<Animator>();
        hen.wanderingDistance = 300;
        hen.startingPosition = new Vector2(transform.position.x, transform.position.y);
        hen.facingRight = false;
        hen.body = this.GetComponent<Rigidbody2D>();
        hen.target = GameObject.FindGameObjectWithTag("Player");
        hen.speed = 70;
        hen.addedScore = false;
        state = State.STATE_WALKING;
        guiManager = GameObject.FindGameObjectWithTag("LevelManager");
        transform.localScale = new Vector2(hen.size, hen.size);
        if (hardcoreBoss)
        {
            guiManager.GetComponent<GuiObserver>().MessageUpdate("We meet again Shiro, but this time... I HAVE EGGS!!!!!");
        }
        else
        {
            guiManager.GetComponent<GuiObserver>().MessageUpdate("Attack the enemy to start!");
        }
        
        
	}
	
	// Update is called once per frame
    new
    void Update()
    {
        base.Update();
        if (hen.target.transform.position.x - 60 > transform.position.x)
        {
           
                hen.facingRight = true;
                flip();
            
        }
        else if (hen.target.transform.position.x + 50 < transform.position.x)
        {
            
                hen.facingRight = false;
                flip();
            
        }

        hen.speed += 0.005f;
        if (hen.health <= 0)
        {
            if (!state.Equals(State.STATE_DEATH))
            {

                changeState(State.STATE_DEATH);
            }
        }
    }

    /*
     *          States
     */

    public override void onIdle()
    {

    }
    public override void onDeath()
    {
        hen.animator.SetBool("idle", false);
        hen.animator.SetBool("dead", true);
        hen.animator.SetBool("walking", false);

        if (LevelScriptController.GetComponent<Boss1>().goingUp != false)
        {
            StopAllCoroutines();
            LevelScriptController.GetComponent<Boss1>().goingUp = false;
        }

        if (!hen.addedScore)
        {
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GuiObserver>().AddScore(3000);
            if (hardcoreBoss)
            {
                guiManager.GetComponent<GuiObserver>().MessageUpdate("NOT AGAINN!!!!!!!");
            }
            else
            {
                guiManager.GetComponent<GuiObserver>().MessageUpdate("AHHH you have beat me, but I shall be back!");
            }
            hen.addedScore = true;
        }
    }
    public override void onWalking()
    {

    }
    public override void onAttacking()
    {
        hen.animator.SetBool("idle", false);
        hen.animator.SetBool("walking", true);
        hen.animator.SetBool("dead", false);



        if (!hen.facingRight)
        {
            float step = hen.speed * Time.deltaTime;
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(hen.target.transform.position.x - 30, transform.position.y), step);
        }
        else if (hen.facingRight)
        {
            float step = hen.speed * Time.deltaTime;
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(hen.target.transform.position.x + 30, transform.position.y), step);
        }
    }


    private void changeState(State status)
    {  
        state = status;
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag.Equals("Player"))
        {
            if (coll.contacts[0].normal != new Vector2(0, -1))
            {
                if (state.Equals(State.STATE_ATTACKING))
                {
                    coll.transform.GetComponent<CharacterController>().character.health -= 10;
                    fallback();
                }
            }



            if (!state.Equals(State.STATE_DEATH))
            {


                if (coll.contacts[0].normal == new Vector2(0, -1))
                {
                    hurt();
                    hen.health -= 25;
                    hen.size -= 4;
                    guiManager.GetComponent<GuiObserver>().updateBossHealth(hen.health);
                    transform.localScale = new Vector2(hen.size, hen.size);
                    Rigidbody2D body = coll.transform.GetComponent<Rigidbody2D>();


                    //Apply velocity to the player script (to make them bounce off)
                    if (coll.gameObject.GetComponent<CharacterController>().isFacingRight)
                    {
                        body.velocity = new Vector2(coll.gameObject.GetComponent<CharacterController>().currentVelocity * body.velocity.x + 400, body.velocity.y + 400);
                    }
                    else
                    {
                        body.velocity = new Vector2(coll.gameObject.GetComponent<CharacterController>().currentVelocity * body.velocity.x + -400, body.velocity.y + 400);
                    }
                    fallback();
                    
                }
            }
        }
        else if (coll.transform.tag.Equals("MagicBlast"))
        {
            if (hen.facingRight)
            {
                hen.body.AddForce(new Vector2(0, -705), ForceMode2D.Impulse);
            }
            else
            {
                hen.body.AddForce(new Vector2(0, 705), ForceMode2D.Impulse);
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("MagicBullet"))
        {
            if (state.Equals(State.STATE_ATTACKING))
            {
               
                hen.health -= 40;
                hen.size -= 16;
                guiManager.GetComponent<GuiObserver>().updateBossHealth(hen.health);
                transform.localScale = new Vector2(hen.size, hen.size);
                hurt();
                if (hen.facingRight)
                {
                    hen.body.AddForce(new Vector2(-1500, 1000), ForceMode2D.Impulse);
                }
                else if (!hen.facingRight)
                {
                    hen.body.AddForce(new Vector2(1500, 1000), ForceMode2D.Impulse);
                }
            }
            else if (state.Equals(State.STATE_WALKING))
            {
                changeState(State.STATE_ATTACKING);

                
                if (hardcoreBoss)
                {
                    GameManager.instance.playSong(4);
                    StartCoroutine("shoot");
                }
                else
                {
                    GameManager.instance.playSong(2);
                }
                guiManager.GetComponent<GuiObserver>().updateBossHealth(hen.health);
            }
            else
            {
                print("no state");
            }
            
        }
    }


    private void fallback()
    {
        if (hen.facingRight)
        {
            hen.body.AddForce(new Vector2(-3500, 1000), ForceMode2D.Impulse);
        }
        else if (!hen.facingRight)
        {
            hen.body.AddForce(new Vector2(3500, 1000), ForceMode2D.Impulse);
        }
    }

    private void flip()
    {
        Vector2 rotation = transform.rotation.eulerAngles;
        if (hen.facingRight)
        {

            rotation.y = 180;
            transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            rotation.y = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    private void hurt()
    {
        if (hen.health > 0)
            GameManager.instance.playSoundEffect(1);
    }

    private void shootEgg()
    {
        GameObject egg = GameObject.Instantiate(EggPrefab, new Vector2(transform.position.x + 0, transform.position.y), transform.rotation) as GameObject;

        if (hen.facingRight)
        {
            egg.GetComponent<EggController>().isRight = true;
        }
        else
        {
            egg.GetComponent<EggController>().isRight = false;
        }
    }


    IEnumerator shoot()
    {
        yield return new WaitForSeconds(1.2f);
        shootEgg();
        StartCoroutine(shoot());
        
    }
}
