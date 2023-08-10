using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static AudioManager;
using static AudioManager.AudioClips;
using static ExtensionMethods;

public class AudioManager : MonoBehaviour
{
    GameObject gameManager;
    GameOver gameOver;

    [SerializeField] AudioSource musicPlayer;

    [SerializeField] AudioSource shipThruster;
    [SerializeField] AudioSource TunneRotate;
    [SerializeField] AudioSource UIHover;
    [SerializeField] AudioSource UIClick;
    [SerializeField] AudioSource ShipExplosion;
    [SerializeField] AudioSource AmbientSpace;

    List<AudioSource> gameSounds = new();

    public enum AudioClips { Thruster, Ambience, SnapRotate, UIHover, UIClick, Explosion, None }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        gameOver = gameManager.GetComponent<GameOver>();

        gameOver.EndGame += OnGameEnd;

        gameSounds.AddMultiple(shipThruster, TunneRotate, UIHover, UIClick, ShipExplosion, AmbientSpace);

        PlayAudioClip(Thruster);

        PlayAudioClip(Ambience);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGameEnd()
    {
        PlayAudioClip(Explosion);
    }

    public void OnClick()
    {
        Debug.Log("Audio Click Played");
        PlayAudioClip(AudioClips.UIClick);
    }

    public void OnEnter()
    {
        PlayAudioClip(AudioClips.UIHover);
    }

    public void PlayAudioClip(AudioClips audioClipType)
    {
        AudioSource audioToPlay = audioClipType switch
        {
            Ambience => AmbientSpace,
            Thruster => shipThruster,
            SnapRotate => TunneRotate,
            AudioClips.UIHover => UIHover,
            AudioClips.UIClick => UIClick,
            Explosion => ShipExplosion,
            _ => null
        };

        if (audioToPlay == null)
        {
            Debug.LogWarning("AudioClip Type not recognized");
            return;
        }

        audioToPlay.Play();
    }

    public void MuteGameSounds(bool muteGame)
    {
        foreach (AudioSource source in gameSounds)
            source.mute = muteGame;
    }

    public void MuteGameMusic(bool muteMusic)
    {
        musicPlayer.mute = muteMusic;
    }
}
