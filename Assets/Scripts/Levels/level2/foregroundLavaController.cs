using UnityEngine;
using System.Collections;

public class foregroundLavaController : MonoBehaviour {
    public GameObject lava;
    public float distance;
    public float speed;
    private Vector2 lavaStartingPosition;
    public bool Invert;
    public bool goingUp;
    private float endingPos;
	// Use this for initialization
	void Start () {
        if (lava)
        {
            lavaStartingPosition = new Vector2(lava.transform.position.x, lava.transform.position.y);
            goingUp = true;
            endingPos = lavaStartingPosition.y + distance;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (lava)
        {
            if (Invert)
            {
                if (!goingUp) //going down
                {
                    float step = speed * Time.deltaTime;
                    lava.transform.position = Vector2.MoveTowards(lava.transform.position, new Vector2(0, lavaStartingPosition.y - distance), step);
                }
                else if (goingUp)
                {
                    float step = speed * Time.deltaTime;
                    lava.transform.position = Vector2.MoveTowards(lava.transform.position, new Vector2(0, lavaStartingPosition.y), step);
                }

                if (lava.transform.position.y >= lavaStartingPosition.y)
                {
                    goingUp = false;

                }
                else if (lava.transform.position.y <= lavaStartingPosition.y - distance)
                {
                    goingUp = true;

                }
            }
            else if (!Invert)
            {
                if (!goingUp)
                {
                    float step = speed * Time.deltaTime;
                    lava.transform.position = Vector2.MoveTowards(lava.transform.position, new Vector2(0, lavaStartingPosition.y), step);
                }
                else if (goingUp)
                {
                    float step = speed * Time.deltaTime;
                    lava.transform.position = Vector2.MoveTowards(lava.transform.position, new Vector2(0, endingPos), step);
                }

                if (lava.transform.position.y >= endingPos)
                {
                    goingUp = false;

                }
                else if (lava.transform.position.y <= lavaStartingPosition.y)
                {
                    goingUp = true;

                }
            }
        }

        
        

        

        


         
        
        
        


        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(endingPos, gameObject.transform.position.y), step);
	}
}
