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
            gameObject.transform.rotation = Quaternion.Euler(0,0,90);
            Vector3 oldPos = gameObject.transform.position;
            oldPos.y -= 0.2f;
            gameObject.transform.position = oldPos;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StateManager.numEnemiesAlive--;
            float velocity = GameplayController.GetVelocity().magnitude;
            Score.Kill(velocity);
        }
    }
}