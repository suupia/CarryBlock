using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickerController : MonoBehaviour
{
    bool isInitialized = false;

    // Pure
    PickerInfo pickerInfo;
    IPickerState pickerState;

    // Components
    GameObject targetResourceObj;
    GameObject headquartersObj;
    float detectionRange;


    public void Init(float rangeRadius)
    {
        Debug.Log($"PickerController.Init() rangeRadius:{rangeRadius}");

        detectionRange = rangeRadius;
        headquartersObj = GameObject.Find("Headquarters");

        pickerInfo = new PickerInfo(this.gameObject);

        Search();


        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        pickerState.Process();
        if (pickerState.CanSwitchState())
        {
            if (pickerState.GetType() == typeof(PickerApproachState)) {
                pickerState = new PickerCollectionState(pickerInfo, this.gameObject, targetResourceObj);
                pickerState.InitProcess();
            }
            else if (pickerState.GetType() == typeof(PickerCollectionState)) {
                pickerState = new PickerReturnState(pickerInfo, this.gameObject, targetResourceObj, headquartersObj);
                pickerState.InitProcess();
            }
            else if (pickerState.GetType() == typeof(PickerReturnState)) {
                pickerState = new PickerApproachState(pickerInfo, this.gameObject, targetResourceObj);
                pickerState.InitProcess();
            }
        }
    }

    private void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Select(collider => collider.gameObject);
        if (resources.Any())
        {
            targetResourceObj = resources.ElementAt(0);
            pickerInfo.SetTargetResourceObj(targetResourceObj);
            pickerState = new PickerApproachState(pickerInfo, this.gameObject, targetResourceObj);
            pickerState.InitProcess();
        }
        else
        {
            pickerState = new PickerNotFoundState(pickerInfo);
            pickerState.InitProcess();
        }

    }

    // Debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}


// State Pattern

public class PickerInfo
{
    public readonly float acceleration = 10f;
    public readonly float maxVelocity = 30;

    public readonly float collectionRange = 1;
    public readonly float collectionTime = 1.5f;
    public readonly float collectOffset = 1;

    public readonly float returnRange = 1;

    public GameObject pickerObj { get; private set;}
    public Rigidbody pickerRd { get; private set;}
    public GameObject targetResourceObj { get; private set;}
    public GameObject headquartersObj { get; private set;}

    public  PickerInfo(GameObject pickerObj)
    {
        this.pickerObj = pickerObj;
        this.pickerRd = pickerObj.GetComponent<Rigidbody>();
    }

    public void SetTargetResourceObj(GameObject targetResourceObj)
    {
        this.targetResourceObj = targetResourceObj;
    }

    public void SetHeadquartersObj(GameObject headquartersObj)
    {
        this.headquartersObj = headquartersObj;
    }
}

public abstract class PickerAbstractState : IPickerState
{
    PickerInfo info;
    GameObject targetResourceObj;
    GameObject headquartersObj;

    public PickerAbstractState(PickerInfo info)
    {
        this.info = info;
    }

    public abstract void InitProcess();
    public abstract void Process();
    public abstract bool CanSwitchState();
}
public class PickerNotFoundState: PickerAbstractState
{

    public PickerNotFoundState(PickerInfo info):base(info)
    {

    }

    public override void InitProcess()
    {

    }
    public override void Process()
    {
    }

    public override bool CanSwitchState()
    {
        return false;
    }

}

public class PickerApproachState : IPickerState
{
    PickerInfo info;
    GameObject pickerObj;
    Rigidbody pickerRd;
    GameObject targetResourceObj;
    public PickerApproachState(PickerInfo info, GameObject pickerObj, GameObject targetResourceObj)
    {
        this.info = info;
        this.pickerObj = pickerObj;
        pickerRd = pickerObj.GetComponent<Rigidbody>();
        this.targetResourceObj = targetResourceObj;
    }

    public void InitProcess()
    {

    }
    public void Process()
    {
        var directionVec = Utility.SetYToZero(targetResourceObj.transform.position - pickerObj.transform.position).normalized;
        pickerRd.AddForce(info.acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= info.maxVelocity) pickerRd.velocity = info.maxVelocity * pickerRd.velocity.normalized;
    }

    public bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(targetResourceObj.transform.position - pickerObj.transform.position);
        return vector.magnitude <= info. collectionRange;
    }

}



public class PickerCollectionState : IPickerState
{
    PickerInfo info;
    GameObject pickerObj;
    Rigidbody pickerRd;
    GameObject targetResourceObj;

    Vector3 initPos;
    Vector3 deltaVector;

    float timer = 0;

    public PickerCollectionState(PickerInfo info, GameObject pickerObj, GameObject targetResourceObj)
    {
        this.info = info;
        this.pickerObj = pickerObj;
        pickerRd = pickerObj.GetComponent<Rigidbody>();
        this.targetResourceObj = targetResourceObj;
    }

    public void InitProcess()
    {
        pickerRd.velocity = Vector3.zero;
        initPos = pickerObj.transform.position;
        deltaVector = targetResourceObj.transform.position - initPos;
    }
    public void Process()
    {
        Debug.Log($"CollectProcess() timer:{timer}");
        timer += Time.deltaTime;
        if (timer < info.collectionTime)
        {
            var coefficient = 2 * Mathf.PI / info. collectionTime;
            var progress = -Mathf.Cos(coefficient * timer) + 1f;
            pickerObj.transform.position = progress * deltaVector + initPos;
        }
        else
        {
            Debug.Log("complete collect");
            targetResourceObj.transform.position = pickerObj. transform.position - new Vector3(0,info. collectOffset, 0);
            targetResourceObj.transform.parent = pickerObj. transform;
        }

    }

    public bool CanSwitchState()
    {
        return timer >= info.collectionTime;
    }



}

public class PickerReturnState : IPickerState
{
    PickerInfo info;
    GameObject pickerObj;
    Rigidbody pickerRd;
    GameObject targetResourceObj;
    GameObject headquartersObj;

    Vector3 initPos;
    Vector3 deltaVector;

    float timer = 0;

    public PickerReturnState(PickerInfo info, GameObject pickerObj, GameObject targetResourceObj, GameObject headquartersObj)
    {
        this.info = info;
        this.pickerObj = pickerObj;
        pickerRd = pickerObj.GetComponent<Rigidbody>();
        this.targetResourceObj = targetResourceObj;
        this.headquartersObj = headquartersObj;
    }

    public void InitProcess()
    {

    }
    public void Process()
    {
        Debug.Log($"ReturnProcess()");
        var directionVec = Utility.SetYToZero(headquartersObj.transform.position - pickerObj. transform.position).normalized;
        pickerRd.AddForce(info.acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= info.maxVelocity) pickerRd.velocity = info.maxVelocity * pickerRd.velocity.normalized;
    }

    public bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(headquartersObj.transform.position - pickerObj.transform.position);
        return vector.magnitude <= info.returnRange;
    }

}


public interface IPickerState
{
    public abstract void InitProcess();
    public abstract void Process();
    public abstract bool CanSwitchState();
}