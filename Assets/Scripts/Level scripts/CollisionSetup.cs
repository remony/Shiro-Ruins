using UnityEngine;
using System.Collections;

public class CollisionSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] lavas = GameObject.FindGameObjectsWithTag("Lava");
        for (int i = 0; i < lavas.Length; i++ )
        {
            if (!lavas[i].name.Equals("Collision"))
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
            if (!grounds[i].name.Equals("Collision"))
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
            if (movingPlatform[i].transform.FindChild("Collision"))
            {
                movingPlatform[i].transform.FindChild("Collision").gameObject.tag = "MovingPlatform";
                movingPlatform[i].AddComponent<MovingPlatformController>();
            }
           
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


        GameObject[] PlatformGrounds = GameObject.FindGameObjectsWithTag("Platforms");
        for (int i = 0; i < PlatformGrounds.Length; i++)
        {
            if (!PlatformGrounds[i].name.Equals("Collision"))
                PlatformGrounds[i].transform.FindChild("Collision").gameObject.tag = "Platforms";
        }

        GameObject[] PlatformGround = GameObject.FindGameObjectsWithTag("Platforms");
        for (int i = 0; i < PlatformGround.Length; i++)
        {
            if (!PlatformGround[i].name.Equals("Collision"))
                PlatformGround[i].transform.FindChild("Collision").gameObject.tag = "Platforms";
        }
        GameObject[] WaterPlatforms = GameObject.FindGameObjectsWithTag("WaterPlatform");
        for (int i = 0; i < WaterPlatforms.Length; i++)
        {
            if (!WaterPlatforms[i].name.Equals("Collision"))
                WaterPlatforms[i].transform.FindChild("Collision").gameObject.tag = "WaterPlatform";
        }

        GameObject[] StairPlatforms = GameObject.FindGameObjectsWithTag("StairsPlatform");
        for (int i = 0; i < StairPlatforms.Length; i++)
        {
            if (!StairPlatforms[i].name.Equals("Collision"))
            {
                StairPlatforms[i].transform.FindChild("Collision").gameObject.tag = "StairsPlatform";
                StairPlatforms[i].transform.FindChild("Collision").GetComponent<PolygonCollider2D>().sharedMaterial = (PhysicsMaterial2D)Resources.Load("GroundPhysicsMaterial");
            }
            
            //StairPlatforms[i].transform.FindChild("Collision").ge
            //.material = (PhysicMaterial)Resources.Load("PhysicMaterials/Wood")
        }


 

            
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
