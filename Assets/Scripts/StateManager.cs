using UnityEngine.SceneManagement;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    enum GameState {Play, Death, Win, Title};
    public static int numEnemiesAlive;

    bool activeGame = false;
    bool m_SceneLoaded;
    public string inputNameSubmit = "Submit";

    GameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        numEnemiesAlive = 0;
        switch(SceneManager.GetActiveScene().name){
            case "Title Screen":
                print("Active scene is " + SceneManager.GetActiveScene().name);
                currentState = GameState.Title;
                break;
            default:
                break;
        }
    }

    void Play(){
        print("Starting Game");
        // set scene to shiplevel
        SceneManager.LoadScene("ShipLevel", LoadSceneMode.Additive);
        // play music looping?
        currentState = GameState.Play;
    }

    void Win(){
        print("you win!!");
        currentState = GameState.Win;
        // play victory theme
    }

    void Death(){
        currentState = GameState.Death;
        // play death theme
    }

    void Title(){
        currentState = GameState.Title;
    }

    // Update is called once per frame
    void Update()
    {
        if(numEnemiesAlive>0 && !activeGame){
            activeGame = true;
        }
        if(numEnemiesAlive == 0 && activeGame){
            Win();
        }

        switch (currentState){
            case GameState.Title:
            {
                if (Input.GetButtonDown(inputNameSubmit)){
                    Play();
                }
                break;
            }
            default:
            {
                break;
            }

            
        }
    }
}
