using UnityEngine;
using System.Collections;

public class Mainmenu : MonoBehaviour {
    
	// Use this for initialization
	void Start () {


        GameManager.instance.playSong(2);
	}
	
	// Update is called once per frame
	void Update () {

        //AudioSource source = this.gameObject.AddComponent<AudioSource>();
        //source.clip = clip;

        //source.Play();


        //AudioSource sound = gameObject.GetComponent<AudioSource>();
        //sound.Play();
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
