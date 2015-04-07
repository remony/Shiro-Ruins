using UnityEngine;
using System.Collections;

public class DebugToolKit : MonoBehaviour {
    public bool displayFPS = false;
    public bool fpsOpen = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (displayFPS)
        {
            if (!fpsOpen)
            {
                this.gameObject.AddComponent<FPSCounter>();
                fpsOpen = true;
            }
            
        }
        else
        {
            Destroy(gameObject.GetComponent("FPSCounter"));
            fpsOpen = false;
        }
	}
}
