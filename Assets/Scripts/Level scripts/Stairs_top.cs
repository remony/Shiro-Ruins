using UnityEngine;
using System.Collections;

public class Stairs_top : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "Stairs_top";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
