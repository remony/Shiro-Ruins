using UnityEngine;
using System.Collections;



public class SignManagerTutorial : MonoBehaviour {
    SignController signController;
    JSONObject signJson = null;
	// Use this for initialization
	void Start () {
        GameObject[] signs = GameObject.FindGameObjectsWithTag("SignPost");

        int count = 0;
        Debug.Log("There is " + signs.Length + " signs");
        

        

        TextAsset mydata = Resources.Load("Signs/Tutorial") as TextAsset;
        signJson = new JSONObject(mydata.text);

        for (int i = 0; i < signs.Length; i++)
        {
            //signController = signs[i].GetComponent<SignController>().sign;
            string text = signJson[i].GetField("text").ToString();
            //signs[i].name = "Sign " + i;
            int id = signs[i].GetComponent<SignController>().sign.id;
            signs[i].GetComponent<SignController>().sign.id = i;
            signs[i].GetComponent<SignController>().sign.text = signJson[id].GetField("text").ToString();
        }
        //Appl
       
        
	}

    
    
	
	// Update is called once per frame
	void Update () {
	
	}
}
