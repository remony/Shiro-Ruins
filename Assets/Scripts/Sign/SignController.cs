using UnityEngine;
using System.Collections;
using System;

public class SignController : SignStateHandler
{

    public String text;
    public int id;
    public Sign sign;
    public GameObject target;
    public GameObject notifyButton;


    

    public override void onIdle()
    {
     
    }

    public override void onPlayer()
    {
       
    }


    // Use this for initialization
    new
    void Start()
    {
        base.Start();
        sign = new Sign();
        try
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject;
        }
        catch (System.Exception e)
        {
            Debug.Log("no player" + e);
        }
        
        sign.id = id;
        sign.text = text;
        sign.read = false;

        sign.guiController = GameObject.FindGameObjectWithTag("LevelManager");
        
    }

    // Update is called once per frame
    new
    void Update()
    {
        base.Update();
        
        float distance = Vector2.Distance(gameObject.transform.position, new Vector2(target.transform.position.x, target.transform.position.y));
        if (distance <15)
        {
            notifyButton.SetActive(true);
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("e"))
            {
                sign.guiController.GetComponent<GuiObserver>().MessageUpdate(sign.text);
            }
        }
        else
        {
            notifyButton.SetActive(false);
        }
    }

    private void stateChanged()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            state = State.STATE_PLAYER;
            
            if (sign.read == false)
            {
                sign.guiController.GetComponent<GuiObserver>().MessageUpdate(sign.text);
                sign.read = true;
            }            
        }

    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            state = State.STATE_PLAYER;            
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            state = State.STATE_IDLE;
        }
    }
}