using System;
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
        DestroyObject().Forget();
    }

    private async UniTaskVoid DestroyObject()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
        Destroy(this.gameObject);
    }
}
