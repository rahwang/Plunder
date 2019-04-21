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
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        y += (float)0.3;

        Instantiate(sailorPrefab, new Vector3(x, y, z), Quaternion.identity);
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
