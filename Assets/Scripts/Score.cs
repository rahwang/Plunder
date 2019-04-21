using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private static Score instance = null;
    int totalScore;
    private bool newScore = false;
    // Start is called before the first frame update
    void Start()
    {   
        Debug.Assert(Score.instance==null);
        Score.instance = this;
        totalScore = 0;
    }

    public static void Kill(int velocity){
        Score.instance.totalScore+=Mathf.RoundToInt(velocity*1000);
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
            newScore = true;
        }

        if (newScore)
        {
            string scoreString = "" + totalScore + " Doubloon";
            if(totalScore != 1){
                scoreString += "s";
            }

            TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
            textmeshPro.SetText(scoreString);

            print("Score: "+totalScore);
        }
    }
}
