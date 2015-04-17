using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class MusicManager : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip audioClip;
    private Rigidbody2D body;

    private bool backgroundMusicPlaying = false;

    GameObject canvas;
    private int currentID = 0;

    JSONObject musicList = null;


    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
        Text[] textValue = canvas.GetComponentsInChildren<Text>();
        textValue[0].text = "";
        //body.position = new Vector2(textValue[0].transform.position.x -200, transform.position.y);
        
        //textValue[0].transform.position = -textValue[0].transform.position;

    }

	void Start () {
        
        audioSource = gameObject.AddComponent<AudioSource>();
        body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        canvas = GameObject.Find("Canvas");
        Text[] text = canvas.GetComponentsInChildren<Text>();
        
        //text[0].transform.position = new Vector2(text[0].transform.position.x - 200, transform.position.y);
        //text[0].horizontalOverflow = OverflowException;
        
        //message = this.gameObject.AddComponent<Text>();
	}

    public void togglePause(int id)
    {
        

        if (backgroundMusicPlaying)
        {
            switch (id)
            {
                //Background music
                case 0:
                    audioSource.Pause();
                    backgroundMusicPlaying = false;
                    
                    break;
            }
        }
        else if (!backgroundMusicPlaying)
        {
            switch (id)
            {
                //Background music
                case 0:
                    audioSource.Play();
                    backgroundMusicPlaying = true;
                    break;
            }
        }
        
    }
    public void playSong(int id)
    {
        Debug.Log("oh");
        currentID = id;
        audioSource.clip = getSong(id);
        audioSource.loop = true;
        //audioSource.PlayOneShot(getSong(id), 1f);
        audioSource.Play();

        updateDisplay(id);
    }

    private void updateDisplay(int id)
    {
        Text[] textValue = canvas.GetComponentsInChildren<Text>();
        textValue[0].text = "Title: " + musicList[id].GetField("title").ToString() + "\nArtist: " + musicList[id].GetField("artist").ToString() + "\nAlbum: " + musicList[id].GetField("album").ToString();
        //body.position = new Vector2(textValue[0].transform.position.x + 200, transform.position.y);
        StartCoroutine("wait");
    }

    AudioClip getSong(int id)
    {
        TextAsset data = Resources.Load("sounds/MusicList") as TextAsset;
        musicList = new JSONObject(data.text);
        AudioClip song = null;

		string location = musicList [id].GetField ("location").str;
        //location = location.Trim(new Char[] { '"' });

        song = Resources.Load(location) as AudioClip;
        //Debug.Log("Playing: " + location);
        return song;
    }


    void displayDetails(int id)
    {

    }
	

   
	// Update is called once per frame
	void Update () {


	}
}
