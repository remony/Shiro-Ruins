using UnityEngine;
using System.Collections;

public class badGuyController : MonoBehaviour {
    public Transform target;
    public float speed = 4f;
    public float distanceLimit = 3f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       transform.Rotate(new Vector3(0, target.position.y, 0), Space.Self);//correcting the original rotation

        transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, - transform.eulerAngles.z);
         //move towards the player
         if (Vector3.Distance(transform.position,target.position)>1f){//move if distance from target is greater than 1
             transform.Translate(new Vector3(0, speed * Time.deltaTime, 0) );
         }
    }
}
