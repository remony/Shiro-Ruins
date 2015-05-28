using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mainmenu : MonoBehaviour {

    public GameObject HardcoreGameModeButton;
    public GameObject scrollbarObject;

	// Use this for initialization
	void Start () {


        GameManager.instance.playSong(7);
        //If the hardcore mode is set to true then the button to access should be shown
        if (PlayerPrefs.GetInt("HCmodeActivated") == 0)
        {
            HardcoreGameModeButton.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("HCmodeActivated") == 1)
        {
            HardcoreGameModeButton.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    

    public void setGameType(int type)
    {
        GameManager.instance.ChangeGameType(type);
    }

    public void changelevel(int level)
    {
        GameManager.instance.changeLevel(level);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
