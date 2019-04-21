using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float swayIntensity = 1.0f;
    public int swayOctaves = 4;
    private float movingAverage = 0.0f;
    public float swaySpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float randomValue = 0.0f;
        for (int i = 0; i < this.swayOctaves; ++i)
        {
            randomValue += UnityEngine.Random.value * 1.0f / (float)(i + 1);
        }

        this.movingAverage += randomValue * swaySpeed * Time.deltaTime;

        float randomAngle = Mathf.Sin(this.movingAverage) * 0.5f * this.swayIntensity * Mathf.PI;


        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * randomAngle);
    }
}
