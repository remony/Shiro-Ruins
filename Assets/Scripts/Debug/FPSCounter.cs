/*
 * 
 * 
 *                  FPS counter -  Opless
 *                  source: http://wiki.unity3d.com/index.php/FramesPerSecond
 * 
 *              Please note that this is included in the project for debugging reasons and not 
 *              anything to do with my project please go to the source as it is all from there.
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
	// Use this for initialization
	void Start () {

        GUIText guiText = this.gameObject.AddComponent<GUIText>();
        guiText.transform.position = new Vector3(0.5f, 0.5f, 0f);

        if (!GetComponent<GUIText>())
        {
            Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
            enabled = false;
            return;
        }
        timeleft = updateInterval; 
	}
	
	// Update is called once per frame
	void Update () {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            GetComponent<GUIText>().text = format;

            if (fps < 30)
                GetComponent<GUIText>().material.color = Color.yellow;
            else
                if (fps < 10)
                    GetComponent<GUIText>().material.color = Color.red;
                else
                    GetComponent<GUIText>().material.color = Color.green;
            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
	}
}
