using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class LevelSelectorController : MonoBehaviour
{
    TextAsset data;
    JSONObject saveData;
    Text[] Gui;
    public GameObject levelButton;
    public GameObject ButtonParent;
    public Text Title;
    public Text SubTitle;

    public List<levelItem> menuList;

    // Use this for initialization
    void Start()
    {
        Gui = gameObject.GetComponentsInChildren<Text>();
        saveData = new JSONObject(PlayerPrefs.GetString("Save"));
        updateGUI(); //populate the list with levels (from json)

        if (GameManager.instance.GetGameType() == 0)
        {
            Title.text = "Adventure";
        }
        else if (GameManager.instance.GetGameType() == 1)
        {
            Title.text = "Time Trial";
        }
        else if (GameManager.instance.GetGameType() == 2)
        {
            Title.text = "Hardcore";
        }

        //SubTitle.text = "Level Selection";


        
        GameManager.instance.playSong(7); 

    }

    private void updateGUI()
    {
        for (int i = 0; i < saveData.list.Count; i++)
        {
            int levelid = 0;
            GameObject buttonObject = Instantiate(levelButton) as GameObject;
            LevelButton button = buttonObject.GetComponent<LevelButton>();
            string message = "";
            //If the level was unlocked
            if (saveData[i].GetField("unlocked").n == 1)
            {
                if (GameManager.instance.GetGameType() == 0 || GameManager.instance.GetGameType() == 2) //Adventure mode or Hardcore mode
                {
                    message = saveData[i].GetField("Title").str + " | Colour restored: " + saveData[i].GetField("ColourPercentage").n + "\nScore: " + saveData[i].GetField("Score").n + " | Time: " + saveData[i].GetField("Time").n;
                
                }
                else if (GameManager.instance.GetGameType() == 1) //Time trial mode
                {
                    message = saveData[i].GetField("Title").str + " | Time: " + saveData[i].GetField("TTTime").n;
                
                }

                if (saveData[i].GetField("conquered").n == 1)
                {
                    message += " [Conquered]";
                }

                button.gameObject.GetComponentInChildren<Text>().text = message;
                levelid = Convert.ToInt32(saveData[i].GetField("id").n);
            }
            else //if the level is not yet locked
            {
                button.gameObject.GetComponentInChildren<Text>().text = saveData[i].GetField("Title").str + " (LOCKED)";
            }


            if (levelid != 0)
            {
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameManager.instance.changeLevel(levelid);
                });
            }

            buttonObject.transform.SetParent(ButtonParent.gameObject.transform);
        }
    }

    public void changeLevel(int id)
    {
        GameManager.instance.changeLevel(id);
    }
}

public class levelItem
{
    public string title;
    public Button.ButtonClickedEvent loadButtonLevel;
}

