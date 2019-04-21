using UnityEngine.SceneManagement;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    enum GameState {Play, Death, Win, Title};

    bool m_SceneLoaded;
    public string inputNameSubmit = "Submit";
    GameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("ShipLevel", LoadSceneMode.Additive);
    }

    void Play(){
        // set scene to shiplevel
        // play music looping?
        currentState = GameState.Play;
    }

    void Win(){
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
