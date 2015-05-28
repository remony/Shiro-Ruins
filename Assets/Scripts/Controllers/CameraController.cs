using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float duration = 0f;
    private Vector3 speed = Vector3.zero;
    public Transform target;
    public GameObject StartPos;
    public GameObject EndPos;

    Vector3 destination;
    //Add padding to the 
    public int DistanceFromLeft = 0;

    CameraStore cameraStore;

    // Use this for initialization
    void Start()
    {
        cameraStore = new CameraStore();

        target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;

        if (StartPos != null && EndPos != null)
        {
            cameraStore.StartPos = StartPos.transform.position;
            cameraStore.EndPos = EndPos.transform.position;
        }
        destination = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z));
            destination = Vector3.zero;
            destination = transform.position + delta;


            if ((target.position.x - cameraStore.StartPos.x) < 190f)
            {
                transform.position = new Vector3(190f, target.position.y, -180);
            }
            else
            {
                transform.position = new Vector3(target.position.x, target.position.y, -180);
            }


           

        }
            
    }
}
