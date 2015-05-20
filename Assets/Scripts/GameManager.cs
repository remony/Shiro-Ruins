using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance = null;
    public LevelManager levelManager;
    public MusicManager musicManager;
    public bool debugMode = false;
    public int debug_level = 0;
    GameManagerStore store;

    public bool updateSaveFromfile = false;

    public int level = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (debugMode)
        {
            level = debug_level;
        }
        store = new GameManagerStore();
        DontDestroyOnLoad(this.gameObject);
        levelManager = GetComponent<LevelManager>();

        //load save into gamemanager

        if (PlayerPrefs.GetString("Save").Equals(""))
        {
            Debug.Log("No save found: loading file save");
            store.saveData = new JSONObject((Resources.Load("Save/Save") as TextAsset).text);
            PlayerPrefs.SetString("Save", store.saveData.ToString());
        }
        else
        {
            store.saveData = new JSONObject(PlayerPrefs.GetString("Save"));
            
        }
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

    public void updateSave()
    {
        PlayerPrefs.DeleteKey("Save");
        store.saveData = new JSONObject((Resources.Load("Save/Save") as TextAsset).text);

        PlayerPrefs.SetString("Save", store.saveData.ToString());
    }

    public void playSong(int id)
    {
        musicManager.playSong(id);
    }

    public void stopSong()
    {
        musicManager.stopSong();
    }

    public void playSoundEffect(int id)
    {
        musicManager.playSoundEffect(id);
    }

    public void stopSoundEffect()
    {
        musicManager.stopSoundEffect();
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

    public JSONObject GetSaveData()
    {
        return store.saveData;
    }

    public void UpdateSaveData(JSONObject save)
    {
        store.saveData = save;
        PlayerPrefs.SetString("Save", store.saveData.ToString());
        PlayerPrefs.Save();
        //File.WriteAllText(Environment.CurrentDirectory + "/Assets/Resources/Save/" + @"\Save.json", save.ToString(true));

    }

    public void ChangeGameType(int type)
    {
        levelManager.setGameType(type);
    }

    public int GetGameType()
    {
        return levelManager.GameType();
    }

    void Update()
    {
        if (updateSaveFromfile)
        {
            PlayerPrefs.SetString("Save", null);
            PlayerPrefs.Save();
            print(PlayerPrefs.GetString("Save"));
            print("PlayerPrefs deleted");
            //store.saveData = new JSONObject((Resources.Load("Save/Save") as TextAsset).text);
            //PlayerPrefs.SetString("Save", store.saveData.ToString());
            PlayerPrefs.Save();
            print(PlayerPrefs.GetString("Save"));
            //UpdateSaveData(store.saveData);
            updateSaveFromfile = false;
        }
        
        
    }

}
