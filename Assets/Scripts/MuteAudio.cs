using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour
{
    public void Mute(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0;
        }
    }

    public void Play(bool Played)
    {
        if (Played)
        {
            AudioListener.volume = 1;
        }
    }
}
