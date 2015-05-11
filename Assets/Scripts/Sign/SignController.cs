using UnityEngine;
using System.Collections;
using System;

public class SignController : SignStateHandler
{

    public String text;
    public int id;
    public Sign sign;
    public GameObject target;
    private GameObject dialogViewer;
    public GameObject notifyButton;


    GameObject guiController;



    
    public override void onIdle()
    {
        //StartCoroutine(wait());
    }

    public override void onPlayer()
    {
        //gameObject.GetComponentInChildren<TextMesh>().text = sign.text;
    }


    // Use this for initialization
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
            Debug.Log("no player");
        }
        
        sign.id = id;
        sign.text = text;
        sign.read = false;

        guiController = GameObject.FindGameObjectWithTag("LevelManager");
        //guiController = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<SignObserver>();
        


        dialogViewer = GameObject.Find("DialogViewer");
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        float distance = Vector2.Distance(gameObject.transform.position, new Vector2(target.transform.position.x, target.transform.position.y));
        if (distance <15)
        {
            notifyButton.SetActive(true);
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("e"))
            {
                guiController.GetComponent<GuiObserver>().MessageUpdate(sign.text);
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
                //dialogViewer.SendMessage("changeMessage", sign.text);
                guiController.GetComponent<GuiObserver>().MessageUpdate(sign.text);
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