using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TimeTester : MonoBehaviour
{

    bool isRunningUniTask;
    bool isRunningCoroutine;
    double testInterval = 0.1;

    double totalUniTaskNoise;
    double totalCoroutineNoise;

    void Start()
    {
        var endPos = new Vector3(0, 0, 10);
    }


    void Update()
    {

        if (!isRunningUniTask && !isRunningCoroutine)
        {
            Debug.Log($"totalUniTaskNoise:{totalUniTaskNoise}, totalCoroutineNoise:{totalCoroutineNoise}, (UniTask - Coroutine):{totalUniTaskNoise - totalCoroutineNoise}");
            StartCollect();
            StartCoroutine(StartTimer());
        }

    }

    async void StartCollect()
    {
        isRunningUniTask = true;
        var before = Time.time;
        var initPos = transform.position;
        for (double t = 0; t < testInterval; t += Time.deltaTime)
        {
            await UniTask.Yield();
        }
        var after = Time.time;
        var interval = after - before;
        Debug.Log($"UniTask wait time:{interval}");
        totalUniTaskNoise += interval;
        isRunningUniTask = false;
    }


    IEnumerator StartTimer()
    {
        isRunningCoroutine = true;
        var before = Time.time;
        for (double t = 0; t < testInterval; t += Time.deltaTime)
        {
            yield return null;
        }
        var after = Time.time;
        var interval = after - before;
        Debug.Log($"Coroutine wait time:{interval}");
        totalCoroutineNoise += interval;
        isRunningCoroutine = false;

    }



}