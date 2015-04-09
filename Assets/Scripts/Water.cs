using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //gameObject.GetComponent<Renderer>().material.color = Color.red;
        transform.FindChild("mesh_[05]_[00]").gameObject.tag = "Water";
        transform.FindChild("Collision").gameObject.tag = "Water";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
