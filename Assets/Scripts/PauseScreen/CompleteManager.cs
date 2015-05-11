using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompleteManager : MonoBehaviour {
    GameObject compelteMenu;
    public GameObject detailsObject;
	// Use this for initialization
	void Start () {
        compelteMenu = GameObject.FindGameObjectWithTag("CompletionScreen");
	}



    public void onContinue()
    {
        Destroy(compelteMenu);
        Time.timeScale = 1;
    }

    public void onLeaveRuins()
    {
        GameManager.instance.changeLevel(2);
        Time.timeScale = 1;
    }

    public void onQuit()
    {
        GameManager.instance.changeLevel(1);
        Time.timeScale = 1;
    }
}
