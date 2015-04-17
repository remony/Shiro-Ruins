using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "Ground";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
