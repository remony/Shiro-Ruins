using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour {

    GameObject pauseMenu;
    GameObject guiController;
	
    // Use this for initialization
	void Start () {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        guiController = GameObject.FindGameObjectWithTag("LevelManager");
	}
    
    public void onContinue()
    {
        Destroy(pauseMenu);
        Time.timeScale = 1;
        guiController.GetComponent<GuiObserver>().ChangeState(2);
    }

    public void onRestart()
    {
        GameManager.instance.changeLevel(GameManager.instance.level);
    }

    public void onOptions()
    {
        //print("Not implemented");
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
