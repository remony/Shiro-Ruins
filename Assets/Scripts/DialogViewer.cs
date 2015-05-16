using UnityEngine;
using System.Collections;

public class DialogViewer : MonoBehaviour {
    private GameObject player;

    private AudioClip NotificationSound;
    private AudioSource audioSource;

    
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        gameObject.GetComponentInChildren<TextMesh>().text = "";

    }

    void changeMessage(string message)
    {
        if (message.Length > 10)
        {
            gameObject.GetComponentInChildren<TextMesh>().fontSize = 50;
        }   else if ( message.Length > 20)
        {
            gameObject.GetComponentInChildren<TextMesh>().fontSize = 35;
        }
        gameObject.GetComponentInChildren<TextMesh>().text = message;
        audioSource.PlayOneShot(NotificationSound, 1f);
        StopCoroutine("wait");
        StartCoroutine("wait");
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponentInChildren<TextMesh>().text = "";
        audioSource = gameObject.AddComponent<AudioSource>();
        NotificationSound = Resources.Load("sounds/Notification") as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector2(player.transform.position.x - 60, player.transform.position.y + 30);
        
	
	}
}
