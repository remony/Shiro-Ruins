using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class LevelManager : MonoBehaviour {

     

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void loadLevel(int level)   {
        switch (level)
        {
            case 0:
                Application.LoadLevel("splashscreen");
                break;
            case 1:
                Application.LoadLevel("MainMenu");
                break;
            case 2:
                Application.LoadLevel("spriteValley");
                break;
            default:
                Application.LoadLevel("MainMenu");
                break;
        }
    }

    public void setupLevel(int level)    {
        loadLevel(level);
    }
}
