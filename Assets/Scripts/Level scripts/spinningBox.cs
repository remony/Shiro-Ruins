using UnityEngine;
using System.Collections;

/*
 * 
 *          Code from and altered
 *          http://unity3d.com/learn/tutorials/modules/beginner/scripting/assignments/spinning-cube
 * 
 */

public class spinningBox : MonoBehaviour {
     
    public float speed = 20f;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(Vector3.right, speed * Time.deltaTime * 1.2f);
	}
}
