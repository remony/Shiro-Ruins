using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Assets.Scripts.Level_Section;

public class LevelSelectorController : MonoBehaviour
{
    
    public GameObject levelButton;
    public GameObject ButtonParent;
    public Text Title;
    public Text SubTitle;

    private LevelSelector levelSelector;

    public List<levelItem> menuList;

    // Use this for initialization
    void Start()
    {
        levelSelector = new LevelSelector();
        levelSelector.saveData = new JSONObject(PlayerPrefs.GetString("Save"));
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
        
        GameManager.instance.playSong(7); 

    }

    void Update()
    {
        playerInput();

    }
    private void playerInput()
    {
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("delete"))
        {
            GameManager.instance.changeLevel(1);
        }
    }


    private void updateGUI()
    {
        for (int i = 0; i < levelSelector.saveData.list.Count; i++)
        {
            int levelid = 0;
            GameObject buttonObject = Instantiate(levelButton) as GameObject;
            LevelButton button = buttonObject.GetComponent<LevelButton>();
            string title = "";
            string score = "";
            string status = "";
            string time = "";
            //If the level was unlocked
            if (levelSelector.saveData[i].GetField("unlocked").n == 1)
            {
                if (GameManager.instance.GetGameType() == 0 || GameManager.instance.GetGameType() == 2) //Adventure mode or Hardcore mode
                {
                    title = levelSelector.saveData[i].GetField("Title").str;
                    score = levelSelector.saveData[i].GetField("Score").n.ToString();
                    time = levelSelector.saveData[i].GetField("Time").n.ToString();
                    if (levelSelector.saveData[i].GetField("conquered").n == 1)
                    {
                        status = "Conquered!";
                    }
                    else
                    {
                        status = " ";
                    }


                    button.gameObject.GetComponentsInChildren<Text>()[0].text = title;
                    button.gameObject.GetComponentsInChildren<Text>()[1].text = status;
                    button.gameObject.GetComponentsInChildren<Text>()[2].text = "Time :" + time;
                    button.gameObject.GetComponentsInChildren<Text>()[3].text = "Score: " + score;
                    //message = saveData[i].GetField("Title").str + " | Colour restored: " + saveData[i].GetField("ColourPercentage").n + "\nScore: " + saveData[i].GetField("Score").n + " | Time: " + saveData[i].GetField("Time").n;
                
                }
                else if (GameManager.instance.GetGameType() == 1) //Time trial mode
                {
                    button.gameObject.GetComponentsInChildren<Text>()[0].text = levelSelector.saveData[i].GetField("Title").str;
                    button.gameObject.GetComponentsInChildren<Text>()[1].text = "";
                    button.gameObject.GetComponentsInChildren<Text>()[2].text = "Time :" + levelSelector.saveData[i].GetField("TTTime").n;
                    button.gameObject.GetComponentsInChildren<Text>()[3].text = "";
                
                }




                levelid = Convert.ToInt32(levelSelector.saveData[i].GetField("id").n);
            }
            else //if the level is not yet locked
            {
                button.gameObject.GetComponentsInChildren<Text>()[0].text = levelSelector.saveData[i].GetField("Title").str;
                button.gameObject.GetComponentsInChildren<Text>()[1].text = "locked";
                button.gameObject.GetComponentsInChildren<Text>()[1].color = Color.red;
                button.gameObject.GetComponentsInChildren<Text>()[2].text = "";
                button.gameObject.GetComponentsInChildren<Text>()[3].text = "";
                button.gameObject.GetComponentsInChildren<Text>()[4].text = "";
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

