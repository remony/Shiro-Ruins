using UnityEngine;
using System.Collections;

public class Mainmenu : MonoBehaviour {

    public GameObject HardcoreGameModeButton;

	// Use this for initialization
	void Start () {


        GameManager.instance.playSong(3);

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

        //AudioSource source = this.gameObject.AddComponent<AudioSource>();
        //source.clip = clip;

        //source.Play();


        //AudioSource sound = gameObject.GetComponent<AudioSource>();
        //sound.Play();
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
