using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to set the camera to Canvas's worldCamera.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CanvasCameraSetter : MonoBehaviour
{
    void Awake()
    {
        var worldCamera = Camera.main;
        if (worldCamera == null)
        {
            Debug.LogError("Main camera is not found");
            return;
        }
        GetComponent<Canvas>().worldCamera = worldCamera;
    }
}
