using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class UniTaskTester : MonoBehaviour
{
    void Start()
    {
        Method();
    }

    async void Method()
    {
        await Wait1Sec();
        Debug.Log($"log");
        await Wait1Sec();
        Debug.Log($"log");
        await Wait1Sec();
        Debug.Log($"log");
        await Wait1Sec();
        Debug.Log($"log");
    }

    // 1ïbë“ã@Ç∑ÇÈUniTask  
    async UniTask Wait1Sec()
    {
        await UniTask.Delay(1000);
    }
}
