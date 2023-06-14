using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SetActiveTester : NetworkBehaviour
{
    [SerializeField]  GameObject gameObjectToSetActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObjectToSetActive.SetActive(!gameObjectToSetActive.activeSelf);
        }
    }
}
