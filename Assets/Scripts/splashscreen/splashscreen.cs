using UnityEngine;
using System.Collections;

public class splashscreen : MonoBehaviour {
    public float delaytime = 5;

    IEnumerator start()
    {
        yield return new WaitForSeconds(delaytime);
        changelevel(1);
    }
	// Update is called once per frame
	void Update () {
        StartCoroutine(start());

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            StopCoroutine(start());
            changelevel(1);
        }

	}


    public void changelevel(int level)
    {
        GameManager.instance.changeLevel(level);
    }
}
