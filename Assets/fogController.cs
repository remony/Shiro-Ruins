using UnityEngine;
using System.Collections;

public class fogController : MonoBehaviour {
    private Vector3 speed = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 destination;
        destination = new Vector3(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.x + 300, transform.position.y, 0) ;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 1f);
        //transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.x, transform.position.y);
	}
}
