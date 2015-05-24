using UnityEngine;
using System.Collections;

public class SharedBehaviour : MonoBehaviour {

    public void hit(int health)
    {

        GameManager.instance.playSoundEffect(1);
    }




    
}
