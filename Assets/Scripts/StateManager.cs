using UnityEngine.SceneManagement;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    enum GameState {Play, Death, Win, Title};
    public static int numEnemiesAlive;

    bool isEnemiesLoaded = false;
    public string inputNameSubmit = "Submit";

    GameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        this.EnsureBootstrapSceneIsLoaded();
        numEnemiesAlive = 0;
        this.EnterTitle();
    }

    void EnsureBootstrapSceneIsLoaded()
    {
        LoadSceneKeepBootstrapUnloadOthers("Bootstrap", "Bootstrap");
    }

    private void SetGameObjectsActive(GameObject[] objects, bool active)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].gameObject.SetActive(active);
        }
    }

     public void LoadSceneKeepBootstrapUnloadOthers(string sceneToLoadName, string sceneToKeepName)
     {
        bool isSceneFound = false;

        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            var scene = SceneManager.GetSceneAt(sceneIndex);
            if (!scene.IsValid()) { continue; }
            if (scene.name.Equals(sceneToKeepName))
            {
                SetGameObjectsActive(scene.GetRootGameObjects(), true);
                if (sceneToLoadName.Equals(sceneToKeepName))
                {
                    return;
                }
                continue;
            }

            this.SetGameObjectsActive(scene.GetRootGameObjects(), false);

            if (scene.name.Equals(sceneToLoadName))
            {
                SetGameObjectsActive(scene.GetRootGameObjects(), true);
                isSceneFound = true;
            }
        }

        if (!isSceneFound)
        {
            SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Additive);
        }
    }


    void EnterTitle()
    {
        Debug.Log("Entering title");
        currentState = GameState.Title;
        this.LoadSceneKeepBootstrapUnloadOthers("Title Screen", "Bootstrap");
    }

    void Title()
    {
        if (Input.GetButtonDown(inputNameSubmit))
        {
            this.EnterPlay();
        }
    }

    void EnterPlay()
    {
        currentState = GameState.Play;
        print("Starting Game");

        this.LoadSceneKeepBootstrapUnloadOthers("ShipLevel", "Bootstrap");
        isEnemiesLoaded = false;
        // play music looping?
    }

    void Play()
    {
        // TODO: Test Death here.

        if (Input.GetButton("Escape"))
        {
            this.EnterTitle();
        }

        if (!isEnemiesLoaded && numEnemiesAlive > 0)
        {
            isEnemiesLoaded = true;
        }


        if (isEnemiesLoaded && numEnemiesAlive <= 0)
        {
            this.EnterWin();
        }

    }

    void EnterWin()
    {
        currentState = GameState.Win;
        print("you win!!");
        this.LoadSceneKeepBootstrapUnloadOthers("Win", "Bootstrap");
    }

    void Win()
    {
        // play victory theme
        if (Input.GetButtonDown(inputNameSubmit))
        {
            this.EnterTitle();
        }
    }

    void EnterDeath()
    {
        currentState = GameState.Death;

        // OpenScene unloads current scene and loads requested scene.
        this.LoadSceneKeepBootstrapUnloadOthers("Death", "Bootstrap");
    }

    void Death()
    {
        if (Input.GetButtonDown(inputNameSubmit))
        {
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
