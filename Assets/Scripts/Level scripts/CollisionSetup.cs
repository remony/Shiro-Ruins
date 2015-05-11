using UnityEngine;
using System.Collections;

public class CollisionSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] lavas = GameObject.FindGameObjectsWithTag("Lava");
        for (int i = 0; i < lavas.Length; i++ )
        {
            lavas[i].transform.FindChild("Collision").gameObject.tag = "Lava";
        }

        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].transform.FindChild("Collision").gameObject.tag = "Ground";
        }

        GameObject[] Stairs_top = GameObject.FindGameObjectsWithTag("Stairs_top");
        for (int i = 0; i < Stairs_top.Length; i++)
        {
            Stairs_top[i].transform.FindChild("Collision").gameObject.tag = "Stairs_top";
            Stairs_top[i].AddComponent<Stairs_top>();
        }

        GameObject[] Stairs = GameObject.FindGameObjectsWithTag("Stairs");
        for (int i = 0; i < Stairs.Length; i++)
        {
            Stairs[i].transform.FindChild("Collision").gameObject.tag = "Stairs";
        }

        GameObject[] Ends = GameObject.FindGameObjectsWithTag("End");
        for (int i = 0; i < Ends.Length; i++)
        {
            if (Ends[i].transform.FindChild("Collision"))
            {
                Ends[i].transform.FindChild("Collision").gameObject.tag = "End";
                Ends[i].transform.FindChild("Collision").gameObject.GetComponent<Collider2D>().isTrigger = false;
            }
                
        }

        GameObject[] movingPlatform = GameObject.FindGameObjectsWithTag("MovingPlatform");
        for (int i = 0; i < movingPlatform.Length; i++)
        {
            movingPlatform[i].transform.FindChild("Collision").gameObject.tag = "MovingPlatform";
            movingPlatform[i].AddComponent<MovingPlatformController>();
        }

 

            
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
