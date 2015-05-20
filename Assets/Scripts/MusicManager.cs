using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MusicManager : MonoBehaviour {
    public AudioMixer Mixer;
    public AudioSource audioSource;
    public AudioSource SoundEffect;
    public AudioClip audioClip;
    public GameObject MusicInfoViewer;
    private GameObject canvas;
    private MusicM mm;
    
    void Start () {

        //Create the music object
        mm = new MusicM();
        TextAsset data = Resources.Load("sounds/MusicList") as TextAsset;
        mm.musicList = new JSONObject(data.text);

        //Set the audiosource for music to loop forever
        audioSource.loop = true;

        //Set the volume of the mixer to the saved volume settings (can be changed by the user in-game)
        Mixer.SetFloat("SEvolume", PlayerPrefs.GetInt("SoundEffectVolume"));
        Mixer.SetFloat("BKvolume", PlayerPrefs.GetInt("MusicVolume"));
           
        canvas = GameObject.Find("Canvas");
	}

    //A method whihc allows you to pause/play the current playing background/music track
    public void togglePause(int id)
    {
        //If the music is playing then pause it 
        if (isMusicPlaying())
        {
            audioSource.Pause();
            mm.musicPlaying = false;
            mm.musicPause = true;
        }
        //If the music is not playing then resume it
        else if (!isMusicPlaying())
        {
            audioSource.Play();
            mm.musicPlaying = true;
            mm.musicPause = false;
        }
    }

    //Play the given sound effect
    public void playSoundEffect(int id)
    {
        if (!currentPlayingSEID().Equals(id))
        {
            mm.playingSoundEffectID = id;
            SoundEffect.clip = getSong(id);
            SoundEffect.Play();
        }
        else
        {
            SoundEffect.Play();
        }
    }

    //Stop the sound effect (possibly not required)
    public void stopSoundEffect()
    {
        SoundEffect.Stop();
    }

    //Play the given id as a song/background track
    public void playSong(int id)
    {
        //If the song is different than the one that is already playing then play the new one
        if (!currentPlayingMusicID().Equals(id))
        {
            mm.playingMusicID = id;
            mm.musicPlaying = true;
            audioSource.clip = getSong(id);
             
            audioSource.Play();
            updateDisplay(id); //Update the display so the user knows what is currently being played.
        }

        
        
    }

    //Stop the current playing song (might not be accessed as songs may change without stopping the previous one, new song replaces)
    public void stopSong()
    {
        audioSource.Stop();
        mm.musicPlaying = false;
    }

    //Displays the song details on the UI
    private void updateDisplay(int id)
    {
        MusicInfoViewer.SetActive(true);
        Text[] textValue = canvas.GetComponentsInChildren<Text>();
        textValue[0].text = "Title: " + mm.musicList[id].GetField("title").ToString() + "\nArtist: " + mm.musicList[id].GetField("artist").ToString() + "\nAlbum: " + mm.musicList[id].GetField("album").ToString();
        StartCoroutine("wait"); // After x seconds make the message disappear
    }


    //Returns the audioclip for the requested song id
    AudioClip getSong(int id)
    {   //Gets the location of the song file from the json and loads it from the resource as an audioClip
        return Resources.Load(mm.musicList [id].GetField ("location").str) as AudioClip;
    }

    //Returns if music is playing or not
    public bool isMusicPlaying()
    {
        return mm.musicPlaying;
    }

    //Returns the id of the current playing song
    public int currentPlayingMusicID()
    {
        return mm.playingMusicID;
    }

    //Returns the id of the current playing sound effect
    public int currentPlayingSEID()
    {
        return mm.playingSoundEffectID;
    }


    //Wait for 10 seconds then set the GUI item to false (so it is hidden)
    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
        MusicInfoViewer.SetActive(false);
    }
}
