using UnityEngine.SceneManagement;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    enum GameState {Play, Death, Win, Title};
    public static int numEnemiesAlive;

    bool m_SceneLoaded;
    public string inputNameSubmit = "Submit";

    GameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START");
        numEnemiesAlive = 0;
        this.EnterTitle();
    }

    void EnterTitle()
    {
        Debug.Log("Entering title");
        currentState = GameState.Title;
        if (SceneManager.GetActiveScene().name != "Title Screen")
        {
            SceneManager.LoadScene("Title Screen", LoadSceneMode.Additive);
        }
    }

    void Title()
    {
        if (Input.GetButtonDown(inputNameSubmit))
        {
            SceneManager.UnloadSceneAsync("Title Screen");
            this.EnterPlay();
        }
    }

    void EnterPlay()
    {
        currentState = GameState.Play;
        print("Starting Game");
        // set scene to shiplevel
        SceneManager.LoadScene("ShipLevel", LoadSceneMode.Additive);
        // play music looping?
    }

    void Play()
    {
        // TODO: Test Death here.


        if (numEnemiesAlive <= 0)
        {
            this.EnterWin();
        }

    }

    void EnterWin()
    {
        currentState = GameState.Win;
        print("you win!!");
        SceneManager.UnloadSceneAsync("ShipLevel");
        SceneManager.LoadScene("Win Screen");
    }

    void Win()
    {
        // play victory theme
        if (Input.GetButtonDown(inputNameSubmit))
        {
            SceneManager.UnloadSceneAsync("Win Screen");
            this.EnterTitle();
        }
    }

    void EnterDeath()
    {
        currentState = GameState.Death;
        SceneManager.UnloadSceneAsync("ShipLevel");
        SceneManager.LoadScene("Death Screen");
    }

    void Death()
    {
        if (Input.GetButtonDown(inputNameSubmit))
        {
            SceneManager.UnloadSceneAsync("Death Screen");
            this.EnterTitle();
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Title: this.Title(); return;
            case GameState.Play: this.Play(); return;
            case GameState.Death: this.Death(); return;
            case GameState.Win: this.Win(); return;
            default: Debug.Assert(false); return;
        }
    }
}
