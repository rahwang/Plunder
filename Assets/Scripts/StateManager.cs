using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    enum GameState {Play, Death, Win, Title};
    GameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        
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
        
    }
}
