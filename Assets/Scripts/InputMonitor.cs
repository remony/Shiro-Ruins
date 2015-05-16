using UnityEngine;
using System.Collections;

public class InputMonitor : MonoBehaviour {

    private GameObject levelManager;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
	}
	
	// Update is called once per frame
	void Update () {
        //check if keyboard
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            levelManager.GetComponent<GuiObserver>().changeInput(0);
            if (Input.GetAxisRaw("Horizontal") >= 0 || Input.GetAxisRaw("Horizontal") <= -0)
            {
                levelManager.GetComponent<GuiObserver>().changeInput(0);
            }
            if (Input.GetAxisRaw("Vertical") >= 0 || Input.GetAxisRaw("Vertical") <= -0)
            {
                levelManager.GetComponent<GuiObserver>().changeInput(0);
            }
        }
        
        //check if controller
        else //(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 3") || Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("joystick button 5") || Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 8") || Input.GetKeyDown("joystick button 9") || )
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKeyDown("joystick button " + i))
                {
                    levelManager.GetComponent<GuiObserver>().changeInput(1);
                }
            }
            if (Input.GetAxisRaw("Horizontal") >= 0.1 && Input.GetAxisRaw("Horizontal") <= 0.9 || Input.GetAxisRaw("Horizontal") <= -0.1 && Input.GetAxisRaw("Horizontal") >= -0.9)
            {
                levelManager.GetComponent<GuiObserver>().changeInput(1);  
            }
            if (Input.GetAxisRaw("Horizontal") >= 0.1 && Input.GetAxisRaw("Horizontal") <= 0.9 || Input.GetAxisRaw("Horizontal") <= -0.1 && Input.GetAxisRaw("Horizontal") >= -0.9)
            {
                levelManager.GetComponent<GuiObserver>().changeInput(1);
            }

                //velocityHorizontal = Input.GetAxisRaw("Horizontal");
                //velocityVertical = Input.GetAxisRaw("Vertical");
        }


        //Checking for controllers connected
        //print(">> there is " + Input.GetJoystickNames().Length + " controllers connected");
        //for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        //{
         //   Debug.Log(Input.GetJoystickNames()[i] + " connected.");
       //}
            
     }

}

