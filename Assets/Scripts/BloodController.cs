using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    // Start is called before the first frame update
    int numTicks;
    bool rightFacing;
    void Start()
    {
        rightFacing = GameplayController.playerIsFacingRight;
        numTicks = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 newPos = this.gameObject.transform.position;
        float rightMod = -1f;
        if (rightFacing)
        {
            rightMod*=-1f;
        }
        newPos.x += 0.03f*rightMod;
        this.gameObject.transform.position=newPos;
        if(numTicks++>15)
        {
            Destroy(this.gameObject);
        }
        
    }
}
