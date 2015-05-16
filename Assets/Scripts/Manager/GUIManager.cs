using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : GUIStateHandle, GuiObserver
{
    public GameObject messageView;
    public GameObject completeView;
    public GameObject failedView;
    public GameObject startCard;
    public GameObject magicCooldown;
    public GameObject controls;

    //Controls sprites references
    public Sprite x_enable;
    public Sprite x_disable;
    public Sprite a_enable;
    public Sprite a_disable;

    //Keyboard sprite references
    public Sprite space_enable;
    public Sprite space_disable;
    public Sprite k_enable;
    public Sprite k_disable;

    public int currentInputType;
    public bool displayHelp = true;

    Text[] textGui;
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
                completeView.GetComponentsInChildren<Text>()[1].text = "Score: " + level.score + "\n Time: " + level.time.ToString("#") + " Seconds.";
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
                failedView.GetComponentsInChildren<Text>()[1].text = "Score: " + level.score + "\n Time: " + level.time.ToString("#") + " Seconds.";
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
                print(saveData()[levelID()].GetField("Title").str);
                

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
                id = 3;
                break;
            case 8:
                id = 4;
                break;
            case 9:
                id = 5;
                break;
            case 10:
                id = 6;
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
                id = 3;
                break;
            case 8:
                id = 4;
                break;
            case 9:
                id = 5;
                break;
            case 10:
                id = 6;
                break;
        }

        if (save[0].GetField("Score").n < level.score)
        {
            save[0].SetField("Score", level.score);
            save[0].SetField("Time", level.time);
            
            if (level.score > 1000)
            {
                save[1].SetField("unlocked", 1);
            }
            GameManager.instance.UpdateSaveData(save);

        }
        else
        {
            print("Score not higher");
        }




    }

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
        textGui[1].text = "Score: " + level.score;
        completeView.SetActive(false);
        failedView.SetActive(false);
        startCard.SetActive(false);
        messageView.SetActive(false);
        controls.gameObject.GetComponent<Animator>().SetBool("display", true);
        displayHelp = true;
    }


    public void OnButtonClick(int levelId)
    {
        GameManager.instance.changeLevel(levelId);
    }

    void Update()
    {
        base.Update();
        timer();
    }

    private void timer()
    {
        textGui[3].text = "Time: " + (Time.time - level.time).ToString("#");
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
        textGui[1].text = "Score: " + level.score;
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
        healthBar.value = level.health;
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
