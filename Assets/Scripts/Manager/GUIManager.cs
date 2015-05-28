using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class GUIManager : GUIStateHandle, GuiObserver
{
    public GameObject messageView;
    public GameObject completeView;
    public GameObject failedView;
    public GameObject startCard;
    public GameObject magicCooldown;
    public GameObject controls;
    public GameObject BossControls;

    //Controls sprites references
    public Sprite x_enable;
    public Sprite x_disable;
    public Sprite a_enable;
    public Sprite a_disable;
    public Sprite b_enable;
    public Sprite b_disable;

    //Keyboard sprite references
    public Sprite space_enable;
    public Sprite space_disable;
    public Sprite k_enable;
    public Sprite k_disable;
    public Sprite l_enable;
    public Sprite l_disable;

    

    public int currentInputType;
    public bool displayHelp = true;

    Text[] textGui;
    new private string name = "";
    Slider healthBar;
    Level level;
    float endTime;

    public override void onProgress()
    {
        //Get input
        input();
    }

    public override void onPause()
    {
        //if there the gameObject PauseMenu doens't exist open the scene
        if (!GameObject.FindGameObjectWithTag("PauseMenu"))
        {
            Application.LoadLevelAdditive("PauseScene");
            Time.timeScale = 0;
        }
        input();
    }

    public override void onUnPause()
    {
        if (GameObject.FindGameObjectWithTag("PauseMenu"))
        {
            GameObject pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            Destroy(pauseMenu);
            Time.timeScale = 1;
            state = State.STATE_INPROGRESS;
        }
        else
        {
            Debug.Log("Could not find Pause menu gameobject :(");
        }
    }

    public override void onComplete()
    {

        if (completeView)
        {
            if (!completeView.activeSelf)
            {
                completeView.SetActive(true);
                
                level.time = Time.time - level.time;

                if (GameManager.instance.GetGameType() == 1)
                {
                    completeView.GetComponentsInChildren<Text>()[1].text = "Game mode: Time Trial";
                    if ((level.time / 60) < 1)
                    {
                        completeView.GetComponentsInChildren<Text>()[4].text = "Highscore: " + saveData()[levelID()].GetField("TTTime").ToString();
                        completeView.GetComponentsInChildren<Text>()[5].text = "Time: " + level.time.ToString("#") + " Seconds.";
                    }
                    else
                    {
                        completeView.GetComponentsInChildren<Text>()[4].text = "Highscore: " + saveData()[levelID()].GetField("TTTime").ToString();
                        completeView.GetComponentsInChildren<Text>()[5].text = "Time: " + (level.time / 60).ToString("#.##") + " Minutes.";
                    }
                    if (level.time > saveData()[levelID()].GetField("TTTime").n)
                    {
                        completeView.GetComponentsInChildren<Text>()[2].text = "Try Again";
                        completeView.GetComponentsInChildren<Text>()[3].text = "";
                    }
                    else if (level.time < saveData()[levelID()].GetField("TTTime").n)
                    {
                        completeView.GetComponentsInChildren<Text>()[2].text = "";
                        completeView.GetComponentsInChildren<Text>()[3].text = "High Score!";
                    }
                }
                else
                {
                    completeView.GetComponentsInChildren<Text>()[1].text = "Game mode: Adventure";

                    if ((level.time / 60) < 1)
                    {
                        completeView.GetComponentsInChildren<Text>()[4].text = "Score: " + level.score;
                        completeView.GetComponentsInChildren<Text>()[5].text = "Time: " + level.time.ToString("#") + " Seconds.";
                    }
                    else
                    {
                        completeView.GetComponentsInChildren<Text>()[4].text = "Score: " + level.score;
                        completeView.GetComponentsInChildren<Text>()[5].text = "Time: " + (level.time / 60).ToString("#.##") + " Minutes.";
                    }



                    if (saveData()[levelID()].GetField("scoreToConquer").n < level.score)
                    {
                        completeView.GetComponentsInChildren<Text>()[2].text = "";
                    }
                    else
                    {
                        completeView.GetComponentsInChildren<Text>()[2].text = "";
                    }

                    
                }
                textGui[4].text = " ";
                textGui[5].text = " ";
                textGui[6].text = " ";
                if (BossControls.activeInHierarchy)
                {
                    BossControls.SetActive(false);
                }
                controls.gameObject.GetComponent<Animator>().SetBool("display", false);
                

                save();
            }
        }
    }

    public override void onDeath()
    {
        if (failedView)
        {
            if (!failedView.activeSelf)
            {
                failedView.SetActive(true);
                level.time = Time.time - level.time;
                if (GameManager.instance.GetGameType() == 0)
                {
                    failedView.GetComponentsInChildren<Text>()[1].text = "Score: " + level.score + "\n Time: " + level.time.ToString("#") + " Seconds.";
                }
                else
                {
                    failedView.GetComponentsInChildren<Text>()[1].text = "Time: " + level.time.ToString("#") + " Seconds.";
                }

            }
        }
    }

    public override void onStart()
    {
        if (startCard)
        {
            if (!startCard.activeSelf)
            {
                startCard.SetActive(true);
                StartCoroutine("displayStart", 2);

                state = State.STATE_INPROGRESS;
                int id = levelID();
                JSONObject save = saveData();
                startCard.GetComponentInChildren<Text>().text = save[id].GetField("Title").str;
            }
        }

        if (magicCooldown)
            if(magicCooldown.activeSelf)
            {
                magicCooldown.SetActive(false);
            }
    }

    IEnumerator displayStart(int delay)
    {
        yield return new WaitForSeconds(delay);
        startCard.SetActive(false);
    }

    public void onRestart()
    {
        GameManager.instance.changeLevel(GameManager.instance.level);
    }

    IEnumerator waitToEnd(int delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.STATE_COMPLETE;
    }

    private int levelID()
    {

        int id = GameManager.instance.level;

        
        switch (id)
        {
            case 3:
                id = 0;
                break;
            case 4: //level 2
                id = 1;
                break;
            case 5:
                id = 2;
                break;
            case 6:
                id = 3;
                break;
            case 7:
                id = 3;
                break;
            case 8: //level 2
                id = 2  ;
                break;
            case 9: //Boss Level 1
                id = 3;
                break;
            case 10: //Boss Level 2
                id = 5;
                break;
            case 11: //Boss Level 2
                id = 4;
                break;
            case 12: //Level 4
                id = 6;
                break;
            case 13: //Credits
                id = 7;
                break;
        }

        return id;
    }

    public JSONObject saveData()
    {
        return new JSONObject(PlayerPrefs.GetString("Save"));
    }

    private void save()
    {
        JSONObject save = saveData();
        int id = levelID();
        /*
        switch (id)
        {
            case 3:
                id = 0;
                break;
            case 4:
                id = 1;
                break;
            case 5:
                id = 2;
                break;
            case 6:
                id = 3;
                break;
            case 7:
                id = 4;
                break;
            case 8:
                id = 3;
                break;
            case 9: //Boss1
                id = 3;
                break;
            case 10: //Boss2
                id = 5;
                break;
            case 11:
                id = 4;
                break;
        }*/

        Debug.Log("Saving ID: " + id);

        
        if (GameManager.instance.GetGameType() == 0 || GameManager.instance.GetGameType() == 2)
        {
            if (save[id].GetField("Score").n < level.score)
            {
                save[id].SetField("Score", level.score);
                save[id].SetField("Time", level.time);
                print(">>>>>> " + level.score + "  |  " + level.time);
                GameManager.instance.UpdateSaveData(save);

            }

            if (save[id].GetField("scoreToConquer").n < level.score)
            {   
                save[id].SetField("conquered", 1);
                save[id + 1].SetField("unlocked", 1);
                GameManager.instance.UpdateSaveData(save);
            }
        }
        else if (GameManager.instance.GetGameType() == 1)
        {
            if (save[id].GetField("TTTime").n > level.time)
            {
                save[id].SetField("TTTime", level.time);
                GameManager.instance.UpdateSaveData(save);
            }
            else
            {
                print("Time not lower");
            }
        }

        




    }
    new
    void Start()
    {
        base.Start();
        level = new Level();
        level.score = 0;
        level.message = "";
        level.time = Time.time;
        textGui = gameObject.GetComponentsInChildren<Text>();
        healthBar = gameObject.GetComponentInChildren<Slider>();
        healthBar.maxValue = 100;
        healthBar.minValue = 0;
        textGui[1].text = level.score.ToString();
        completeView.SetActive(false);
        failedView.SetActive(false);
        startCard.SetActive(false);
        messageView.SetActive(false);
        controls.gameObject.GetComponent<Animator>().SetBool("display", true);
        displayHelp = true;
        BossControls.SetActive(false);
    }


    public void OnButtonClick(int levelId)
    {
        GameManager.instance.changeLevel(levelId);
    }

    new 
    void Update()
    {
        base.Update();
        timer();
    }


    private void timer()
    {
        try
        {
            if (GameManager.instance.GetGameType() == 1)
            {
                textGui[2].text = (Time.time - level.time).ToString("#");
                textGui[1].text = "";
            }
            else
            {
                textGui[2].text = (Time.time - level.time).ToString("#");
            }
        }
        catch(Exception e)
        {
            print(e);
        }
        
        
    }

    private void input()
    {
        if (Input.GetKeyDown("i") || Input.GetKeyDown("joystick button 7"))
        {
            if (Time.timeScale == 1)
            {
                state = State.STATE_PAUSED;
            }
            else
            {
                state = State.STATE_UNPAUSED;
            }
        }
        if (Input.GetKeyDown("h") || Input.GetKeyDown("joystick button 6"))
        {
            displayHelp = !displayHelp;
            if (displayHelp)
            {
                controls.gameObject.GetComponent<Animator>().SetBool("display", true);
            }
            else
            {
                controls.gameObject.GetComponent<Animator>().SetBool("display", false);
            }
            
        }
    }

    public void ChangeState(int id)
    {
        switch (id)
        {
            case 0:
                state = State.STATE_INPROGRESS;
                break;
            case 1:
                state = State.STATE_PAUSED;
                break;
            case 2:
                state = State.STATE_UNPAUSED;
                break;
            case 3:
                StartCoroutine("waitToEnd", 1);
                break;
            case 4:
                state = State.STATE_DEATH;
                break;
        }
    }

    public void MessageUpdate(string message)
    {
        level.message = message;
        displayMessage();
    }

    public void AddScore(int value)
    {
        level.score += value;
        if (GameManager.instance.GetGameType() == 0)
            textGui[1].text = level.score.ToString();
    }

    private void displayMessage()
    {
        messageView.SetActive(true);
        messageView.GetComponentsInChildren<Text>()[0].text = level.message;
        StopCoroutine("disableMessage");
        StartCoroutine("disableMessage");
    }

    IEnumerator disableMessage()
    {
        yield return new WaitForSeconds(5);
        messageView.SetActive(false);
    }

    public void displayLastMessage()
    {
        displayMessage();
    }

    public void UpdateHealth(int health)
    {
        level.health = health;
        textGui[0].text = level.health.ToString();
        healthBar.value = health;
        
    }

    public void updateSphereCooldown(float count)
    {
        if (count < 0.91f)
        {

            controls.GetComponentsInChildren<Text>()[2].text = "Blast (" + (100 - count * 100).ToString("#") + ")";
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].sprite = l_disable;
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].sprite = b_disable;
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
        }
        else if (count >= 0.91f)
        {
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].sprite = l_enable;
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].sprite = b_enable;
                controls.GetComponentsInChildren<Text>()[2].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            controls.GetComponentsInChildren<Text>()[2].text = "Blast";
            magicCooldown.SetActive(false);
        }
    }

    public void updateMagicCooldown(float count)
    {
        
        if (count < 0.91f)
        {

            controls.GetComponentsInChildren<Text>()[1].text = "Magic (" + (100 - count * 100).ToString("#") + ")";
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].sprite = k_disable;
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].sprite = x_disable;
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }

            magicCooldown.SetActive(true);
            magicCooldown.GetComponentsInChildren<Image>()[1].fillAmount = count;
        }
        else if (count >= 0.91f)
        {
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].sprite = k_enable;
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].sprite = x_enable;
                controls.GetComponentsInChildren<Text>()[1].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(2, 2);
            }
            controls.GetComponentsInChildren<Text>()[1].text = "Magic";
            magicCooldown.SetActive(false);
        }
    }

    public void changeGrounded(bool grounded)
    {
        
        if (grounded)
        {
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].sprite = space_enable;
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].sprite = a_enable;
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }


        }
        else
        {
            if (currentInputType == 0)
            {
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].sprite = space_disable;
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }
            else if (currentInputType == 1)
            {
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].sprite = a_disable;
                controls.GetComponentsInChildren<Text>()[0].GetComponentsInChildren<Image>()[0].transform.localScale = new Vector2(1, 1);
            }

        }
        
    }

    public void updateBossHealth(int health)
    {
        if (!BossControls.activeInHierarchy)
        {
            BossControls.SetActive(true);
            int id = levelID();
            JSONObject save = saveData();
            name = save[id].GetField("Title").str;
            BossControls.GetComponentInChildren<Text>().text = save[id].GetField("Title").str;
            BossControls.GetComponentInChildren<Slider>().minValue = 0;
            BossControls.GetComponentInChildren<Slider>().maxValue = 1000;
            BossControls.GetComponentInChildren<Slider>().minValue = 0;
            BossControls.GetComponentInChildren<Slider>().wholeNumbers = true;
        }
        else
        {
            if (health <= 0)
            {
                BossControls.GetComponentInChildren<Slider>().value = 0;
                //BossControls.GetComponentInChildren<Slider>().
            }
            else
            {
                BossControls.GetComponentInChildren<Slider>().value = health;

            }
            BossControls.GetComponentInChildren<Text>().text = name + " | health: " + health;
        }

        
    }

    //Changes the int type of the input (0 = keyboard, 1 = controller)
    public void changeInput(int type)
    {

        if (type == 0)
        {
            currentInputType = 0;
        }
        else if (type == 1)
        {
            currentInputType = 1;
        }
    }

    void OnLevelWasLoaded()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }
}
