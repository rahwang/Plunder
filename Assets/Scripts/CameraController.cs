using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform = null;
    public float playerRadius = 1.0f;
    public Vector2 cameraPositionMin = Vector2.zero;
    public Vector2 cameraPositionMax = Vector2.zero;

    void Awake()
    {
        Debug.Assert(this.transform != null);
        Debug.Assert(playerRadius > 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPositionNext = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            this.transform.position.z
        );

        this.transform.position = cameraPositionNext;
    }
}
