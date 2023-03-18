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
    bool isFixedUpdating;

    [SetUp]
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

    [TearDown]
    public void Teardown()
    {
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var obj in objects)
        {
            Object.Destroy(obj);
        }
    }


    [UnityTest]
    public IEnumerator SearchProcess_FixUpdate_ShouldMovePicker() => UniTask.ToCoroutine(async () =>
    {
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

    });

    //[Test]
    //public void TestApproachState()
    //{
    //    var playerInfo = new PlayerInfo();
    //    var infoWrapper = new PlayerInfoWrapper(playerInfo);

    //    var  pickerInfo = new PickerInfo(pickerObj, infoWrapper);
    //    pickerInfo.SetPlayerObj(playerObj);
    //    pickerInfo.SetMainBaseObj(mainBaseObj);

    //    var pickerContext = new PickerContext(pickerInfo.searchState);




    //    // Set up approach state
    //    var approachState = new PickerApproachState(pickerInfo);

    //    // Set up test variables
    //    var startPosition = pickerObj.transform.position;
    //    var endPosition = resourceObj.transform.position;

    //    // Call InitProcess
    //    approachState.InitProcess();

    //    // Call Process
    //    pickerContext.CurrentState().Process(pickerContext);

    //    // Check if picker moves towards resource
    //    Assert.AreNotEqual(startPosition, pickerObj.transform.position);

    //    // Move picker to resource object
    //    pickerObj.transform.position = endPosition;

    //    // Call Process again
    //    pickerContext.CurrentState().Process(pickerContext);

    //    // Check if picker detects resource and changes state
    //    //Assert.AreEqual(pickerInfo.collectionState, pickerContext.CurrentState());
    //}

    void StartFixedUpdate(Action action, float time)
    {
        TestFixedUpdate(action, time);
    }

    async void TestFixedUpdate(Action action, float time)
    {
        isFixedUpdating = true;
        for (double t = 0; t < time; t += Time.deltaTime)
        {
            action();
            await UniTask.Yield();
        }
        isFixedUpdating = false;
    }
}