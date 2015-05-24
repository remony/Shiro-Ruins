using UnityEngine;
using System.Collections;

public class splashscreen : MonoBehaviour {
    public float delaytime = 5;
    public bool song = true;
    public GameObject splash1;
    public GameObject splash2;
    private int splash = 0;

	// Use this for initialization


    IEnumerator splash1Wait()
    {
        yield return new WaitForSeconds(3.6f);
        displaySplash2();
    }

    IEnumerator splash2Wait()
    {
        yield return new WaitForSeconds(5);
        changelevel(1);
       
    }
	// Update is called once per frame
	void Update () {
        
        
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("space"))
        {
            if (splash == 0)
            {
                StopCoroutine(splash1Wait());
                displaySplash2();
            } 
            else if (splash == 1)
            {
                StopCoroutine(splash2Wait());
                changelevel(1);
            }
            
            //StopCoroutine(astart());
        }

	}


    void displaySplash2()
    {
        splash++;
        splash1.SetActive(false);
        splash2.SetActive(true);
        StartCoroutine(splash2Wait());
    }

  
    void Start()
    {
        GameManager.instance.playSoundEffect(4);
        StartCoroutine(splash1Wait());

        
    }


    
    public void changelevel(int level)
    {
        GameManager.instance.stopSoundEffect();
        GameManager.instance.changeLevel(level);
        
        
    }
}
