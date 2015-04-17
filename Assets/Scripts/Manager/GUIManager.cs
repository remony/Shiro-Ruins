using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GameObject target;
    public int health = 0;
    public bool displayScore = true;
    public int score = 0;
    Text[] healthDisplay;
    CharacterController characterController;

    public GameObject pauseMenu;


	// Use this for initialization
	void Start () {
        healthDisplay = gameObject.GetComponentsInChildren<Text>();
        target = GameObject.FindGameObjectWithTag("Player").gameObject;
        characterController = target.GetComponent<CharacterController>();

        healthDisplay[0].text = "Health: " + health;
        healthDisplay[1].text = "Score: " + score; 
        
	}
	
	// Update is called once per frame
	void Update () {

        if (characterController.character.health != health)
        {
            health = characterController.character.health;
            healthDisplay[0].text = "Health: " + health; 

        }
        if (displayScore)
        {
            healthDisplay[1].text = "Score: " + score;
        }
        else
        {
            healthDisplay[1].text = "";
        }

        if (Input.GetKeyDown("i") ||  Input.GetKeyDown("joystick button 7"))
        {
            if (Time.timeScale == 1)
            {
                Application.LoadLevelAdditive("PauseScene");
                Time.timeScale = 0;
            }
            else
            {
                GameObject pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
                Destroy(pauseMenu);
                Time.timeScale = 1;
            }
            
        }
        
        
	}

    public void AddScore(int value)
    {
        score += value;
    }

    void OnLevelWasLoaded()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }
}
