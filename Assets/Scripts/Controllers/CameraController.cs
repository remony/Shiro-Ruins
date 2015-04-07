using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float duration = 0.15f;
    private Vector3 speed = Vector3.zero;
    public Transform target;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            if (target.transform.position.x > 170)
            {
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
                Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.6f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, duration);
            }
            }
            
    }
}
