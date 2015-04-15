using UnityEngine;
using System.Collections;

public class fun : MonoBehaviour {
    GameObject player;
    private Vector3 speed = Vector3.zero;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y -20);
	}
}
