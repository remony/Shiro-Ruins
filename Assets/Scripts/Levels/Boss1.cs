using UnityEngine;
using System.Collections;
using System;

public class Boss1 : MonoBehaviour {
    public GameObject object1;
    public GameObject object2;
    public float distance;
    public float speed;
    private Vector2 object1StartingPosition;
    public bool Invert;
    public bool goingUp;
    private float endingPos;
    public bool musicPlaying;
	// Use this for initialization
	void Start () {
        
        if (object1 && object2)
        {
            object1StartingPosition = new Vector2(object1.transform.position.x, object1.transform.position.y);
            endingPos = object1StartingPosition.y - distance;
        }
        musicPlaying = true;
        
        try
        {
            
            //GameManager.instance.playSong(3);
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't play music " + e);
        }
	}

    void Update()
    {

        if (object1)
        {
            if (object1.transform.position.y >= object1StartingPosition.y)
            {
                musicPlaying = false;
                try
                {
                    GameManager.instance.stopSong();
                }
               catch (Exception e)
                {
                    print(e);
                }
            }
            else if (object1.transform.position.y >= endingPos - distance)
            {
                
                musicPlaying = true;
            }
            if (Invert)
            {
                if (!goingUp) //going down
                {
                    if (object1.transform.position.y <= object1StartingPosition.y - distance)
                    {
                        float step = speed * Time.deltaTime;
                        object1.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, object1StartingPosition.y - distance), step);
                        object2.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, object1StartingPosition.y - distance), step);

                    }
                }
                else if (goingUp)
                {
                    if (object1.transform.position.y >= object1StartingPosition.y)
                    {
                        float step = speed * Time.deltaTime;
                        object1.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, endingPos), step);
                        object2.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, endingPos), step);
                    }
                }
            }
            else if (!Invert)
            {
                if (!goingUp)
                {
                    if (object1.transform.position.y <= object1StartingPosition.y)
                    {
                        float step = speed * Time.deltaTime;
                        object1.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, object1StartingPosition.y), step);
                        object2.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, object1StartingPosition.y), step);
                        if (musicPlaying)
                        {
                            GameManager.instance.playSoundEffect(1);
                        }
                        
                    }
                }
                else if (goingUp)
                {
                    if (object1.transform.position.y >= endingPos)
                    {
                        float step = speed * Time.deltaTime;
                        object1.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, endingPos), step);
                        object2.transform.position = Vector2.MoveTowards(object1.transform.position, new Vector2(0, endingPos), step);
                    }
                }
            }

            

        }
        
    }
}
