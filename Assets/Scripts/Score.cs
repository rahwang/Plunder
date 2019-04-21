using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    void Kill(int velocity){
        totalScore+=Mathf.RoundToInt(velocity*1000);
    }
    int GetScore(){
        return totalScore;
    }
    // Update is called once per frame
    void Update()
    {
        totalScore-=1;
    }
}
