using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "Stairs";
	}

    void OnTriggerStay2D(Collider2D coll)
    {
        print("Whats up");
    }
}


