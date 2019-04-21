using UnityEngine;
using System.Collections;

public class SailorController : MonoBehaviour
{

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cutlass"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            StateManager.numEnemiesAlive--;
            float velocity = GameplayController.GetVelocity().magnitude;
            Score.Kill(velocity);
        }
    }
}