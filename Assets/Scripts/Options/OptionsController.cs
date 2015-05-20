using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour {

    public Button musicVolumeButton;
    public Button SoundEffectVolume;
    public Button globalSoundVolume;
    public AudioMixerGroup MusicVolume;
    public AudioMixerGroup SEVolume;
    public AudioMixerGroup GlobalVolume;
    public GameObject HCButton; 


    private int[] volumeOptions = { -80, -70, -60, -50, -40, -30, -20, -10, 0 };
    private int[] volumes = { 0, 10, 20, 30, 40, 50, 70, 80, 100 };
    private Option option;




	// Use this for initialization
	void Start () {
        option = new Option();
        option.musicVolume = PlayerPrefs.GetInt("MusicVolume");
        option.soundEffectVolume = PlayerPrefs.GetInt("SoundEffectVolume");

        bool changed = false;
        for (int i = 0; i < volumeOptions.Length; i++)
        {
            if (volumeOptions[i].Equals(option.musicVolume))
            {
                if (!changed)
                {
                    musicVolumeButton.GetComponentInChildren<Text>().text = "Music Volume: " + volumes[i] + "%.";
                }
            }
        }
        changed = false;
        
        for (int i = 0; i < volumeOptions.Length; i++)
        {
            if (volumeOptions[i].Equals(option.soundEffectVolume))
            {
                if (!changed)
                {
                    SoundEffectVolume.GetComponentInChildren<Text>().text = "Sound Effect Volume: " + volumes[i] + "%.";
                }
            }
        }

        if (PlayerPrefs.GetInt("HCmodeActivated") == 0)
        {
            HCButton.GetComponentInChildren<Text>().text = "Hardcore mode [Disabled]";
        }
        else if (PlayerPrefs.GetInt("HCmodeActivated") == 1)
        {
            HCButton.GetComponentInChildren<Text>().text = "Hardcore mode [Activated]";
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //adjust the volume of the given id where id 0 = music volume and id 1 = sound effect volume
    public void adjustVolume(int id)
    {
        bool changed = false;
        if (id == 0)
        {
            for (int i = 0; i < volumeOptions.Length; i++)
            {
                if (volumeOptions[i].Equals(option.musicVolume))
                {
                    if (!changed)
                    {
     
                        //changeVolume = true;
                        if (option.musicVolume.Equals(0))
                        {
                            option.musicVolume = volumeOptions[0];
                            musicVolumeButton.GetComponentInChildren<Text>().text = "Music Volume: " + volumes[0] + "%.";
                            
                        }
                        else
                        {
                            option.musicVolume = volumeOptions[i + 1];
                            musicVolumeButton.GetComponentInChildren<Text>().text = "Music Volume: " + volumes[i + 1] + "%.";
                        }

                        
                        MusicVolume.audioMixer.SetFloat("BKvolume", option.musicVolume);
                        PlayerPrefs.SetInt("MusicVolume", option.musicVolume);
                        changed = true;
                    }


                }

            }
        }
        else if (id == 1)
        {
            for (int i = 0; i < volumeOptions.Length; i++)
            {
                if (volumeOptions[i].Equals(option.soundEffectVolume))
                {
                    if (!changed)
                    {
                        //changeVolume = true;
                        if (option.soundEffectVolume.Equals(0))
                        {
                            SoundEffectVolume.GetComponentInChildren<Text>().text = "Sound Effect Volume: " + volumes[0] + "%.";
                            option.soundEffectVolume = volumeOptions[0];
                        }
                        else
                        {
                            SoundEffectVolume.GetComponentInChildren<Text>().text = "Sound Effect Volume: " + volumes[i+1] + "%.";
                            option.soundEffectVolume = volumeOptions[i + 1];
                        }

                        
                        
                        SEVolume.audioMixer.SetFloat("SEvolume", option.soundEffectVolume);
                        PlayerPrefs.SetInt("SoundEffectVolume", option.soundEffectVolume);
                        changed = true;
                    }


                }

            }
        }
        changed = false;
    }

    public void toggleHardcoreCode()
    {
        
        if (PlayerPrefs.GetInt("HCmodeActivated") == 0)
        {
            PlayerPrefs.SetInt("HCmodeActivated", 1);
            HCButton.GetComponentInChildren<Text>().text = "Hardcore mode [Activated]";
        }
        else if (PlayerPrefs.GetInt("HCmodeActivated") == 1)
        {
            PlayerPrefs.SetInt("HCmodeActivated", 0);
            HCButton.GetComponentInChildren<Text>().text = "Hardcore mode [Disabled]";
        }
    }

    public void resetSave()
    {
        GameManager.instance.updateSave();
    }


    public void changeLevel (int level)
    {
        GameManager.instance.changeLevel(level);
    }
}
