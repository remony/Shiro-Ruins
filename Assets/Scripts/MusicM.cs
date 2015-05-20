using UnityEngine;
using System.Collections;

public class MusicM
{
    public int playingMusicID
    {
        get;
        set;
    }

    public int playingSoundEffectID
    {
        get;
        set;
    }

    public bool musicPlaying
    {
        get;
        set;
    }

    public bool musicPause
    {
        get;
        set;
    }

    public JSONObject musicList
    {
        get;
        set;
    }
}
