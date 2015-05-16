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

        GameObject[] MovingPlatforms = GameObject.FindGameObjectsWithTag("MovingPlatform");
        for (int i = 0; i < MovingPlatforms.Length; i++)
        {
            //MovingPlatforms[i].transform.FindChild("Collision").gameObject.tag = "MovingPlatform";
            //JumpPads[i].AddComponent<BoxCollider2D>();
            //MovingPlatforms[i].transform.FindChild("Collision").gameObject.AddComponent<MovingPlatformController>();
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

        GameObject[] Spikes = GameObject.FindGameObjectsWithTag("Spike");
        for (int i = 0; i < Spikes.Length; i++)
        {
            Spikes[i].transform.FindChild("Collision").gameObject.tag = "Spike";
        }
       

        GameObject[] JumpPads = GameObject.FindGameObjectsWithTag("JumpPad");
        for (int i = 0; i < JumpPads.Length; i++)
        {
            //JumpPads[i].transform.FindChild("Collision").gameObject.tag = "JumpPad";
            //JumpPads[i].AddComponent<BoxCollider2D>();
            //JumpPads[i].transform.FindChild("Collision").gameObject.AddComponent<JumpPadController>();
        }

        


        GameObject[] PlatformGround = GameObject.FindGameObjectsWithTag("Platforms");
        for (int i = 0; i < PlatformGround.Length; i++)
        {
            PlatformGround[i].transform.FindChild("Collision").gameObject.tag = "Platform";
        }


 

            
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
