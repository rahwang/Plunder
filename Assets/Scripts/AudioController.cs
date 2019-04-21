using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip deathClip;
    public AudioClip winClip;
    public AudioClip cutlassClip;
    public AudioClip yarClip;
    public AudioClip yarrClip;
    public AudioClip yarrrClip;
    
    public AudioSource mainSource;
    public AudioSource auxSource;

    void playWin()
    {
        this.mainSource.clip = winClip;
        this.mainSource.Play();
        this.mainSource.loop = false;
    }

    void playDeath()
    {
        this.mainSource.clip = deathClip;
        this.mainSource.Play();
        this.mainSource.loop = false;
    }

    void playYar()
    {
        var x = Random.value;
        if (x < 0.33f) {
            this.auxSource.clip = yarrrClip;
        } else if (x < 0.67f) {
            this.auxSource.clip = yarClip;
        } else {
            this.auxSource.clip = yarrClip;
        }
        this.auxSource.Play();
    }

    void playCutlass()
    {
        this.auxSource.clip = cutlassClip;
        this.auxSource.Play();
    }
}