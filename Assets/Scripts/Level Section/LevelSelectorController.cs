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

    public List<levelItem> menuList;

    // Use this for initialization
    void Start()
    {
        Gui = gameObject.GetComponentsInChildren<Text>();
        saveData = GameManager.instance.GetSaveData();
        updateGUI(); //populate the list with levels (from json)
        GameManager.instance.playSong(7); 

    }

    private void updateGUI()
    {
        for (int i = 0; i < saveData.list.Count; i++)
        {
            int levelid = 0;
            GameObject newButton = Instantiate(levelButton) as GameObject;
            LevelButton button = newButton.GetComponent<LevelButton>();
            if (saveData[i].GetField("unlocked").n == 1)
            {
                button.gameObject.GetComponentInChildren<Text>().text = saveData[i].GetField("Title").str + " | Colour restored: " + saveData[i].GetField("ColourPercentage").n + "\nScore: " + saveData[i].GetField("Score").n + " | Time: " + saveData[i].GetField("Time").n;
                levelid = Convert.ToInt32(saveData[i].GetField("id").n);
            }
            else
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
            
            /*
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    //changeLevel(Convert.ToInt32(saveData[i].GetField("id").n));
                    print(Convert.ToInt32(saveData[i].GetField("id").n));
                    //GameManager.instance.changeLevel(Convert.ToInt32(saveData[i].GetField("id").n));
                });
            
            */
            newButton.transform.SetParent(ButtonParent.gameObject.transform);
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

