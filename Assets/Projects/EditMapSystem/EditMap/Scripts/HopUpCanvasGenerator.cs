using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopUpCanvasGenerator : MonoBehaviour
{
    [SerializeField] GameObject hopUpCanvas;

    public void PopMessage(string text)
    {
        GameObject canvas = Instantiate(hopUpCanvas);
        canvas.GetComponent<HopUpCanvasCon>().SetText(text);
    }
}
