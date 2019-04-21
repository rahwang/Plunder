using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip cutlassClip;
    public AudioClip yarClip;
    public AudioClip yarrClip;
    public AudioClip yarrrClip;
    
    public AudioSource auxSource;

    void Update() {
        var x = Random.value;
        if (x > 0.997f) {
            playYar();
        }
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

    public void playCutlass()
    {
        this.auxSource.clip = cutlassClip;
        this.auxSource.Play();
    }
}