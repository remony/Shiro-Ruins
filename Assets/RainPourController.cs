using UnityEngine;
using System.Collections;

public class RainPourController : MonoBehaviour {

    private Vector3 speed = Vector3.zero;
    float height;
    // Use this for initialization
    void Start()
    {
        height = Camera.main.orthographicSize * 2;
    }

    // Update is called once per frame
    void Update()
    {

        height = Camera.main.orthographicSize * 2;

        gameObject.transform.localScale = Vector3.one * height;

       
        Vector3 destination;
        destination = new Vector3(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.x, GameObject.FindGameObjectWithTag("Player").gameObject.transform.position.y + 300, 0);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 1f);
	}
}
