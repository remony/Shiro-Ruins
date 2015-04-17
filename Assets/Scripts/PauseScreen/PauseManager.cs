using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour {
    GameObject pauseMenu;
	// Use this for initialization
	void Start () {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
	}



    public void onContinue()
    {
        Destroy(pauseMenu);
        Time.timeScale = 1;
    }

    public void onRestart()
    {
        print("Not implemented");
    }

    public void onOptions()
    {
        print("Not implemented");
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
