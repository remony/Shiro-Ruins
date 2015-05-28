using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Slider progressBar;
    public GameObject loadingScreen;
    private int GameTypeID = 0;

    private AsyncOperation async;

    private void loadLevel(int level)   {
        loadingScreen.SetActive(true);
        String sceneName;
        print("LEVEL: " + level);
        switch (level)
        {
                
            case 0:
                sceneName = "splashscreen";
                break;
            case 1:
                sceneName = "MainMenu";
                break;
            case 2:
                sceneName = "LevelSelection";
                break;
            case 3:
                sceneName = "Tutorial";
                break;
            case 4:
                sceneName = "Level1";
                break;
            case 5:
                sceneName = "Options";
                break;
            case 6:
                sceneName = "Help";
                break;
            case 7:
                sceneName = "Credits";
                break;
            case 8:
                sceneName = "level2";
                break;
            case 9:
                sceneName = "Boss1";
                break;
            case 10:
                sceneName = "Boss2";
                break;
            case 11:
                sceneName = "Level3";
                break;
            case 12:
                sceneName = "Level4";
                break;
            case 13:
                sceneName = "Credits";
                break;
            default:
                sceneName = "MainMenu";
                break;
        }

        StartCoroutine(StartLoadingScreen(sceneName));
    }



    public void setupLevel(int level)    {
                loadLevel(level);
    }

    IEnumerator StartLoadingScreen(string sceneName)
    {
        async = Application.LoadLevelAsync(sceneName);
        while (!async.isDone)
        {
            progressBar.value = async.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);

    }

    //Returns the value of the GameTypeID this is the game mode that the user is player 0 = adventure 1 = time trial
    public int GameType()
    {
        return GameTypeID;
    }

    public void setGameType(int type)
    {
        GameTypeID = type;
    }

}
