using UnityEngine;
using System.Collections;

public class CrossJunction : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "CrossJunction";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
