using UnityEngine;
using System.Collections;
using System;

public class SignController : SignStateHandler
{

    public String text;
    public int id;
    public Sign sign;
    private GameObject dialogViewer;

    
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
        //sign.id = Convert.ToInt32(gameObject.name);
        //sign.text = "something";
//        this.gameObject.GetComponentInChildren<TextMesh>().text = "";
        // text = sign.text;
        sign.id = id;

        dialogViewer = GameObject.Find("DialogViewer");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
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
            dialogViewer.SendMessage("changeMessage", sign.text);
            //dialogViewer.changeMessage = sign.text;
            
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.ToString().Equals("Player"))
        {
            state = State.STATE_IDLE;
            //StartCoroutine(wait());
        }
    }
}