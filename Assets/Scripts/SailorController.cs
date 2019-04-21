using UnityEngine;
using System.Collections;

public class SailorController : MonoBehaviour
{
    public GameObject bloodPrefab;
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

            float x = this.transform.position.x;
            float y = this.transform.position.y+0.3f;
            float z = this.transform.position.z;

            GameObject blood = Instantiate(bloodPrefab, new Vector3(x, y, z)  ,Quaternion.identity);
            Vector3 newTrans = blood.transform.localScale;
            if(!GameplayController.playerIsFacingRight)
            {
                newTrans.x *= -1;
            }
            blood.transform.localScale = newTrans;
            blood.transform.parent = this.transform;
        }
    }
}