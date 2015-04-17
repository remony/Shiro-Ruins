using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Slider progressBar;
    public GameObject loadingScreen;

    private AsyncOperation async;

    private void loadLevel(int level)   {
        loadingScreen.SetActive(true);
        String sceneName;
        
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
                sceneName = "spriteValley";
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
            print(async.progress);
            yield return null;
        }
        loadingScreen.SetActive(false);

    }
}
