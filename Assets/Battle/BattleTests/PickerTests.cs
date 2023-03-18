using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;


public class PickerControllerTests
{
    GameObject playerObj;
    GameObject pickerObj;
    GameObject resourceObj;
    GameObject mainBaseObj;
    PlayerInfo playerInfo;

    public void Setup()
    {
        playerObj = new GameObject("Player");
        playerObj.AddComponent<Rigidbody>();
        playerInfo = new PlayerInfo();
        playerInfo.Init(playerObj);

        resourceObj = new GameObject("Resource");
        resourceObj.tag = "Resource";

        mainBaseObj = new GameObject("MainBase");
        mainBaseObj.tag = "MainBase";

        pickerObj = new GameObject("Picker");
        pickerObj.AddComponent<Rigidbody>();
    }

    public void Teardown()
    {
        Object.Destroy(playerObj);
        Object.Destroy(resourceObj);
        Object.Destroy(mainBaseObj);
        Object.Destroy(pickerObj);
    }


    [UnityTest]
    public IEnumerator SearchProcess_FixUpdate_ShouldMovePicker() => UniTask.ToCoroutine(async () =>
    {
        Setup();

        // SetUp scene
        var startPosition = new Vector3(0, 0, 0);
        var endPosition = new Vector3(0, 0, 10);
        pickerObj.transform.position = startPosition;
        resourceObj.transform.position = endPosition;

        // SetUp domain
        var pickerInfo = new PickerInfo(pickerObj, new PlayerInfoWrapper(playerInfo));
        pickerInfo.SetPlayerObj(playerObj);
        pickerInfo.SetMainBaseObj(mainBaseObj);
        var searchState = new PickerSearchState(pickerInfo);
        var pickerContext = new PickerContext(new PickerSearchState(pickerInfo)); // Not used in testing.


        // Check if picker moves different position.
        for (double t = 0; t < 0.1f; t += Time.deltaTime)
        {
            Debug.Log($"time:{Time.time}");
            searchState.InitProcess();
            searchState.Process(pickerContext);
            await UniTask.Yield();
        }

        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

        Teardown();
    });

    [UnityTest]
    public IEnumerator SearchProcess_ReachResource_ShouldChangeStateApproach() => UniTask.ToCoroutine(async () =>
    {
        Setup();

        // SetUp scene
        var startPosition = new Vector3(0, 0, 0);
        var endPosition = new Vector3(0, 0, 0);
        pickerObj.transform.position = startPosition;
        resourceObj.transform.position = endPosition;

        // SetUp domain
        var pickerInfo = new PickerInfo(pickerObj, new PlayerInfoWrapper(playerInfo));
        pickerInfo.SetPlayerObj(playerObj);
        pickerInfo.SetMainBaseObj(mainBaseObj);
        var searchState = new PickerSearchState(pickerInfo);
        var pickerContext = new PickerContext(new PickerSearchState(pickerInfo)); // Not used in testing.


        // Check if picker moves different position.
        for (double t = 0; t < 0.1f; t += Time.deltaTime)
        {
            searchState.Process(pickerContext);
            await UniTask.Yield();
        }

        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

        Teardown();
    });
}