using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#nullable enable

public class OptionCanvasSwitcher : MonoBehaviour
{
    [SerializeField] Button openOptionButton;
    [SerializeField] Button closeOptionButton;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject optioncanvas;
    [SerializeField] GameObject canvasContainer;
    

    void Update()
    {
        openOptionButton.onClick.AddListener(OpenOptionCanvas);
        closeOptionButton.onClick.AddListener(CloseOptionCanvas);
    }

    void OpenOptionCanvas()
    {
        canvasContainer.SetActive(!canvasContainer.activeSelf);
        optioncanvas.SetActive(optioncanvas.activeSelf);
    }

    void CloseOptionCanvas()
    {
        canvasContainer.SetActive(canvasContainer.activeSelf);
        optioncanvas.SetActive(!optioncanvas.activeSelf);
    }
}
