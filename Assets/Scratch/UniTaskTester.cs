using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor.Experimental;
using Time = UnityEngine.Time;

public class UniTaskTester : MonoBehaviour
{

    bool isCollecting;
    float collectTime = 3f;

    Vector3 deltaVector;

    float correctTimer;
    int counter;


    float totalUniTaskNoise;
    float totalCoroutineNoise;

    void Start()
    {
       // Method();

       var endPos = new Vector3(0, 0, 10);
       deltaVector = endPos - transform.position;
    }


    void FixedUpdate()
    {

        if (!isCollecting)
        {
            Debug.Log($"counter:{counter}, correctTimer:{correctTimer}");
            Debug.Log($"totalUniTaskNoise:{totalUniTaskNoise}, totalCoroutineNoise:{totalCoroutineNoise}, (UniTask - Coroutine):{totalUniTaskNoise- totalCoroutineNoise}");
            counter++;
            StartCollect();
            StartCoroutine(StartTimer());
        }

        correctTimer += Time.fixedDeltaTime;
    }

    async void StartCollect()
    {
        isCollecting = true;
        var before = Time.time;
        var initPos = transform.position;
        for (float t = 0; t < collectTime; t+= Time.deltaTime)
        {
            var coefficient = 2 * Mathf.PI / collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;
            transform.position = progress * deltaVector + initPos;
            await UniTask.Yield();
            if(t>= collectTime )Debug.LogWarning($"t:{t}");
        }

        var after = Time.time;
        var interval = after - before;
        Debug.Log($"UniTask wait time:{interval}");
        totalUniTaskNoise += interval;
        isCollecting = false;
    }


    IEnumerator StartTimer()
    {
        var before = Time.time;
        for (float t = 0; t < collectTime; t += Time.deltaTime)
        {
            yield return null;
        }
        var after = Time.time;
        var interval = after - before;
        Debug.Log($"Coroutine wait time:{interval}");
        totalCoroutineNoise += interval;
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

    // 1秒待機するUniTask  
    async UniTask Wait1Sec()
    {
        await UniTask.Delay(1000);
    }

}



