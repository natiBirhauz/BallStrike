using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AudioManager : MonoBehaviour
{
    public AudioClip MenuMusicSource;
    public AudioClip MusicGameLoopSource;
    public AudioClip clickSource;
    public AudioClip pauseSource;
    public AudioClip levelUpSource;
    public AudioClip gameOverSource;
    public AudioClip startGameSource;

    [Header("Ball Sounds")]
    public AudioClip helthBallSource;
    public AudioClip iceBallSource;
    public AudioClip fireBallSource;
    public AudioClip magnetBallSource;
    public AudioClip redBallSource;
    public AudioClip pointsBallSource;
    public AudioClip StarSource;

    [Header("manage Sounds")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public bool isMusicOn = true;
    public bool isSfxOn = true;

    public static AudioManager Instance;

    void Awake()
    {
        if (transform.parent == null)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (musicSource != null && MenuMusicSource != null)
        {
            musicSource.clip = MenuMusicSource;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null && isSfxOn)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip )
    {
        if (musicSource != null && clip != null && isMusicOn)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        if (musicSource != null)
        {
            isMusicOn = false;
            musicSource.Stop();
        }
    }
    public void gameMusic()
    {
        if (musicSource != null && MusicGameLoopSource != null)
        {
            musicSource.clip = MusicGameLoopSource;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
public TextMeshProUGUI musicXText; // 

internal void ToggleMusic()
{
    if (musicSource != null)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            isMusicOn = false;

            if (musicXText != null)
                musicXText.gameObject.SetActive(true); 
        }
        else
        {
            musicSource.Play();
            isMusicOn = true;

                if (musicXText != null)
                    musicXText.gameObject.SetActive(false); //  Show X
        }
    }
}

    public Sprite soundOnSprite;   // ButtonsStyle10_23
    public Sprite soundOffSprite;  // ButtonsStyle10_22
    public RawImage soundToggleRawImage; 

  public void ToggleSFX()
    {
        if (soundToggleRawImage == null)
        {
            Debug.LogError("RawImage is not assigned.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogError("sfxSource is not assigned.");
            return;
        }

        isSfxOn = !isSfxOn;

        if (isSfxOn)
        {
            sfxSource.UnPause();
            soundToggleRawImage.texture = soundOnSprite.texture;
        }
        else
        {
            sfxSource.Pause();
            soundToggleRawImage.texture = soundOffSprite.texture;
        }

        Debug.Log("SFX toggled: " + isSfxOn);
    }

    internal void Stopvfx()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
            isSfxOn = false;

        }
    }
        internal void playVfx()
    {
        if (sfxSource != null)
        {
            sfxSource.Play();
            isSfxOn = true;

        }
    }
}
