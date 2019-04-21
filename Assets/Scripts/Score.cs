using UnityEngine;
using TMPro;

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
        if(1==Random.Range(0,30))
        {
            totalScore-=1;
        }
        string scoreString = "" + totalScore + " Doubloon";
        if(totalScore != 1){
            scoreString += "s";
        }

        TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.SetText(scoreString);
    }
}
