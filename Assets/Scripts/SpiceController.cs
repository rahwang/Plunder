using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceController : MonoBehaviour
{
    private static SpiceController instance = null;

    [Tooltip("1 == most reponsive, 0.0 == most dampening"), Range(0.0f, 1.0f)]
    public float spice = 1.0f;

    void Awake()
    {
        Debug.Assert(SpiceController.instance == null);
        SpiceController.instance = this;
    }

    public static float GetSpice()
    {
        Debug.Assert(SpiceController.instance != null);
        return SpiceController.instance.spice;
    }
}
