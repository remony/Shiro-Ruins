using UnityEngine;
using System.Collections;



public class SignManagerTutorial : MonoBehaviour {
    SignController signController;
    JSONObject signJson = null;
	// Use this for initialization
    void Start()
    {
        GameObject[] signs = GameObject.FindGameObjectsWithTag("SignPost");

        TextAsset mydata = Resources.Load("Signs/Tutorial") as TextAsset;
        signJson = new JSONObject(mydata.text);

        for (int i = 0; i < signs.Length; i++)
        {
            string text = signJson[i].GetField("text").ToString();
            int id = signs[i].GetComponent<SignController>().sign.id;
            signs[i].GetComponent<SignController>().sign.id = i;
            signs[i].GetComponent<SignController>().sign.text = signJson[id].GetField("text").ToString();
        }
    }
}
