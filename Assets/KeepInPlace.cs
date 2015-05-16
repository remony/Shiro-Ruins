using UnityEngine;
using System.Collections;

public class KeepInPlace : MonoBehaviour {
    Rigidbody2D body;
    Transform transformzzzz;
	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();
        transformzzzz = gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        body.velocity = new Vector2(0, 0);
        // body.velocity.y = 0;
        this.gameObject.transform.position = transformzzzz.position;
        print("oh");
        body.gravityScale = 0.0f;
    }
}
