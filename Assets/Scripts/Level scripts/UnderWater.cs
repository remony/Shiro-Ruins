using UnityEngine;
using System.Collections;

public class UnderWater : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.FindChild("Collision").gameObject.tag = "UnderWater";
	}
}
