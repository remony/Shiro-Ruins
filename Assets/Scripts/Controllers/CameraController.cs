using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float duration = 0f;
    private Vector3 speed = Vector3.zero;
    public Transform target;
    public GameObject StartPos;
    public GameObject EndPos;


    //Add padding to the 
    public int DistanceFromLeft = 0;

    CameraStore camera;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        camera = new CameraStore();

        if(GameObject.FindGameObjectsWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        }

        if (StartPos != null && EndPos != null)
        {
            camera.StartPos = StartPos.transform.position;
            camera.EndPos = EndPos.transform.position;
        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z));
            Vector3 destination = Vector3.zero;
            if (target.transform.position.x < camera.StartPos.x + 400)
            {
                destination = new Vector3(camera.StartPos.x + 300, transform.position.y, 0) + delta;
            }
            else if (target.transform.position.x > camera.StartPos.x + 400)
            {

                destination = transform.position + delta;


            }
            else if (target.transform.position.x > camera.StartPos.x - 400)
            {
                if (target.transform.position.x < camera.EndPos.x - 400)
                {
                    destination = new Vector3(camera.EndPos.x - 300, transform.position.y, 0) + delta;
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, duration);
        }
            
    }
}
