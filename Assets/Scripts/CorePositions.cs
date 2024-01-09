using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePositions : MonoBehaviour
{
    public static CorePositions instance;

    public bool Core1Enabled;
    public Vector3 Core1;
    public bool Core2Enabled;
    public Vector3 Core2;

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Awake()
    {
        CreateSingleton();
    }
}
