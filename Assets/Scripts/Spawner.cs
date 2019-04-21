using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sailorPrefab;
    void Start()
    {
        SpawnSailor();
    }

    void SpawnSailor(){
        if (StateManager.numEnemiesAlive > 2)
        {
            return;
        }
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        y += (float)0.3;

        GameObject sailor = Instantiate(sailorPrefab, new Vector3(x, y, z), Quaternion.identity);
        // Make sure sailor is a child of the boat so that they sway with the boat.
        sailor.transform.parent = this.transform.parent;
        StateManager.numEnemiesAlive++;
    }

    // Update is called once per frame
    void Update()
    {
        if( 3 == Random.Range(0,8000))
        {
            // SpawnSailor();
        }

    }
}
