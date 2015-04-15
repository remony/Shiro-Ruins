using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "Ladder";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
