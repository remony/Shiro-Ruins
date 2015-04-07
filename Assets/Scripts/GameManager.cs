using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance = null;
    public LevelManager levelManager;
    //public MusicManager musicManager;
    public AudioSource audioSource;

    private int level = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        audioSource.Play();
        audioSource.time = 1f;

        DontDestroyOnLoad(this.gameObject);
        levelManager = GetComponent<LevelManager>();
        InitGame();
    }

    void OnLevelWasLoaded(int index)
    {
        if (level == 1)
        {
            audioSource.time = 14.4f;
        }

        if (level >= 2)
            audioSource.Stop();
    }

    void InitGame()
    {
        Debug.Log("Loading level " + level);
        levelManager.setupLevel(level);
        
    }

    public void changeLevel(int level)
    {
        this.level = level;
        InitGame();
    }
   
}
