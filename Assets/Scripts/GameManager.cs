using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance = null;
    public LevelManager levelManager;
    public MusicManager musicManager;

    public int level = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        

        DontDestroyOnLoad(this.gameObject);
        levelManager = GetComponent<LevelManager>();
        musicManager = GetComponent<MusicManager>();
        InitGame();
    }

    void OnLevelWasLoaded(int index)
    {
        
    }

    void InitGame()
    {
        Debug.Log("Loading level " + level);
        levelManager.setupLevel(level);
        
    }

    public void playSong(int id)
    {
        musicManager.playSong(id);
    }

    public void togglePauseMusic(int id)
    {
        musicManager.togglePause(id);
    }

    public void changeLevel(int level)
    {
        this.level = level;
        InitGame();
    }
   
}
