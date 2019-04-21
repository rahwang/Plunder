using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip cutlassClip;
    public AudioClip yarClip;
    public AudioClip yarrClip;
    public AudioClip yarrrClip;
    
    public AudioSource auxSource1;
    public AudioSource auxSource2;

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
            this.auxSource1.clip = yarrrClip;
        } else if (x < 0.67f) {
            this.auxSource1.clip = yarClip;
        } else {
            this.auxSource1.clip = yarrClip;
        }
        this.auxSource1.Play();
    }

    public void playCutlass()
    {
        this.auxSource2.clip = cutlassClip;
        this.auxSource2.Play();
    }
}