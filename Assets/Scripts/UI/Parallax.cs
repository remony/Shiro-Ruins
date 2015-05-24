using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
    public GameObject Layer1;
    public GameObject Layer2;
    public GameObject Layer3;
    public GameObject Layer4;
    public GameObject Layer5;

    private GameObject mainCamera;
    private GameObject layoutPosition;

    public float Layer1Speed;
    public float Layer2Speed;
    public float Layer3Speed;
    public float Layer4Speed;
    public float Layer5Speed;


    private float Layer1position;
    private float Layer2position;
    private float Layer3position;
    private float Layer4position;
    private float Layer5position;

    public Vector3 screenPosition = new Vector3(0, 0, 100);

	// Use this for initialization
	void Start () {
        //layoutPosition.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {


        //Layer1.transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x, layoutPosition.transform.position.y);


        Layer2position += Layer2Speed;
        //Layer2.transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - Layer2position, GameObject.FindGameObjectWithTag("Player").transform.position.y);

        Layer3position += Layer3Speed;
        //Layer3.transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - Layer3position, GameObject.FindGameObjectWithTag("Player").transform.position.y);

        Layer4position += Layer4Speed;
        //Layer4.transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - Layer4position, GameObject.FindGameObjectWithTag("Player").transform.position.y);

        Layer5position += Layer5Speed;
        //Layer5.transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - Layer5position, GameObject.FindGameObjectWithTag("Player").transform.position.y);

	}
}
