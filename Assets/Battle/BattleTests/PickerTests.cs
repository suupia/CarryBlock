using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class PickerControllerTests
{
    GameObject playerObj;
    GameObject pickerObj;
    GameObject resourceObj;
    GameObject mainBaseObj;
    PickerController pickerController;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("Player");
        playerObj.AddComponent<Rigidbody>();

        resourceObj = new GameObject("Resource");
        resourceObj.tag = "Resource";

        mainBaseObj = new GameObject("MainBase");
        mainBaseObj.tag = "MainBase";

        pickerObj = new GameObject("Picker");
        pickerObj.AddComponent<Rigidbody>();
        pickerController = pickerObj.AddComponent<PickerController>();
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up objects
        Object.Destroy(playerObj);
        Object.Destroy(pickerObj);
        Object.Destroy(resourceObj);
        Object.Destroy(mainBaseObj);
    }

    [Test]
    public void TestSearchState()
    {
        var pickerInfo = new PickerInfo(pickerObj, 5f);
        pickerInfo.SetPlayerObj(playerObj);
        pickerInfo.SetMainBaseObj(mainBaseObj);

        var pickerContext = new PickerContext(pickerInfo.searchState);
        var searchState = new PickerSearchState(pickerInfo);

        //pickerController.Init(playerObj, pickerInfo.);


        var startPosition = pickerObj.transform.position;
        var endPosition = resourceObj.transform.position;
        Debug.Log($"pickerPos:{startPosition}, resourcePos:{endPosition}");

        // Call InitProcess
        searchState.InitProcess();

        // Call Process
        pickerContext.CurrentState().Process(pickerContext);

        // Check if picker moves
        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

        //// Move picker to end position
        //pickerObj.transform.position = endPosition;

        //// Call Process again
        //pickerContext.CurrentState().Process(pickerContext);

        //// Check if picker detects resource and changes state
        //Assert.AreEqual(pickerInfo.approachState, pickerContext.CurrentState());
    }

    [Test]
    public void TestApproachState()
    {
        var  detectionRange = 5.0f;

        var  pickerInfo = new PickerInfo(pickerObj, detectionRange);
        pickerInfo.SetPlayerObj(playerObj);
        pickerInfo.SetMainBaseObj(mainBaseObj);

        var pickerContext = new PickerContext(pickerInfo.searchState);




        // Set up approach state
        var approachState = new PickerApproachState(pickerInfo);

        // Set up test variables
        var startPosition = pickerObj.transform.position;
        var endPosition = resourceObj.transform.position;

        // Call InitProcess
        approachState.InitProcess();

        // Call Process
        pickerContext.CurrentState().Process(pickerContext);

        // Check if picker moves towards resource
        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

        // Move picker to resource object
        pickerObj.transform.position = endPosition;

        // Call Process again
        pickerContext.CurrentState().Process(pickerContext);

        // Check if picker detects resource and changes state
        //Assert.AreEqual(pickerInfo.collectionState, pickerContext.CurrentState());
    }

}