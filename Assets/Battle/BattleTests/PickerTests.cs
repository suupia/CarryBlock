//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;


//public class PickerControllerTests
//{
//    private GameObject playerObj;
//    private GameObject pickerObj;
//    private GameObject resourceObj;
//    private GameObject mainBaseObj;

//    [SetUp]
//    public void Setup()
//    {
//        // Create player object
//        playerObj = new GameObject("Player");
//        playerObj.AddComponent<Rigidbody>();

//        // Create picker object
//        pickerObj = new GameObject("Picker");
//        pickerObj.AddComponent<Rigidbody>();

//        // Create resource object
//        resourceObj = new GameObject("Resource");
//        resourceObj.tag = "Resource";

//        // Create headquarters object
//        mainBaseObj = new GameObject("Headquarters");

//        // Set up PickerController
//        var pickerController = pickerObj.AddComponent<PickerController>();
//        pickerController.Init(playerObj, 5f);
//    }

//    [TearDown]
//    public void Teardown()
//    {
//        // Clean up objects
//        Object.Destroy(playerObj);
//        Object.Destroy(pickerObj);
//        Object.Destroy(resourceObj);
//        Object.Destroy(mainBaseObj);
//    }

//    [Test]
//    public void TestSearchState()
//    {
//        // Set up PickerInfo
//        var pickerInfo = new PickerInfo(pickerObj, 5f);
//        pickerInfo.SetPlayerObj(playerObj);
//        pickerInfo.SetHeadquartersObj(mainBaseObj);

//        // Set up PickerContext
//        var pickerContext = new PickerContext(pickerInfo.searchState);

//        // Set up search state
//        var searchState = new PickerSearchState(pickerInfo);

//        // Set up test variables
//        var startPosition = pickerObj.transform.position;
//        var endPosition = resourceObj.transform.position;
//        Debug.Log($"pickerPos:{startPosition}, resourcePos:{endPosition}");

//        // Call InitProcess
//        searchState.InitProcess();

//        // Call Process
//        pickerContext.CurrentState().Process(pickerContext);

//        // Check if picker moves
//        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

//        //// Move picker to end position
//        //pickerObj.transform.position = endPosition;

//        //// Call Process again
//        //pickerContext.CurrentState().Process(pickerContext);

//        //// Check if picker detects resource and changes state
//        //Assert.AreEqual(pickerInfo.approachState, pickerContext.CurrentState());
//    }

//    [Test]
//    public void TestApproachState()
//    {
//        // Set up PickerInfo
//        var pickerInfo = new PickerInfo(pickerObj, 5f);
//        pickerInfo.SetPlayerObj(playerObj);
//        pickerInfo.SetHeadquartersObj(mainBaseObj);

//        // Set up PickerContext
//        var pickerContext = new PickerContext(pickerInfo.approachState);

//        // Set up approach state
//        var approachState = new PickerApproachState(pickerInfo);

//        // Set up test variables
//        var startPosition = pickerObj.transform.position;
//        var endPosition = resourceObj.transform.position;

//        // Call InitProcess
//        approachState.InitProcess();

//        // Call Process
//        pickerContext.CurrentState().Process(pickerContext);

//        // Check if picker moves towards resource
//        Assert.AreNotEqual(startPosition, pickerObj.transform.position);

//        // Move picker to resource object
//        pickerObj.transform.position = endPosition;

//        // Call Process again
//        pickerContext.CurrentState().Process(pickerContext);

//        // Check if picker detects resource and changes state
//        Assert.AreEqual(pickerInfo.collectionState, pickerContext.CurrentState());
//    }

//}