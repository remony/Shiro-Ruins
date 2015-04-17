using UnityEngine;
using System.Collections;

public class splashscreen : MonoBehaviour {
    public float delaytime = 5;

    IEnumerator astart()

    {
        yield return new WaitForSeconds(delaytime);
        changelevel(1);
    }
	// Update is called once per frame
	void Update () {
        
        
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            StopCoroutine(astart());
            changelevel(1);
        }

	}

    void Start()
    {
        StartCoroutine(astart());
        GameManager.instance.playSong(3);
    }


    
    public void changelevel(int level)
    {
        GameManager.instance.changeLevel(level);
        
    }
}
