using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HopUpCanvasCon : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hopUpText;

    public void SetText(string text)
    {
        hopUpText.text = text;
    }
    // Start is called before the first frame update
    void Start()
    {
        DestroyObject();
    }

    private async void DestroyObject()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
        Destroy(this.gameObject);
    }
}
