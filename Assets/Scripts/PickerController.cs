using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickerController : MonoBehaviour
{
    bool isInitialized = false;

    // Pure
    PickerInfo pickerInfo = new ();
    IPickerState pickerState;

    // Componets
    Rigidbody pickerRd;
    GameObject targetResourceObj;
    GameObject headquartersObj;
    float detectionRange;

    float collectionRange = 1;
    float collectionTime = 2f;
    float collectOffset = 1;
    Vector3 collectInitPos;
    Vector3 deltaVector;

    float timer = 0;

    public enum PickerState
    {
        NotFound, Approach, Collect, Return
    }

    public PickerState state = PickerState.Approach;

    public void Init(float rangeRadius)
    {
        Debug.Log($"PickerController.Init() rangeRadius:{rangeRadius}");

        pickerRd = GetComponent<Rigidbody>();
        detectionRange = rangeRadius;
        headquartersObj = GameObject.Find("Headquarters");


        Search();

        pickerState = new PickerApproachState(pickerInfo, this.gameObject, targetResourceObj);
        pickerState.InitProcess();


        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;
        //switch (state)
        //{
        //    case PickerState.NotFound:

        //        break;
        //    case PickerState.Approach:
        //        //ApproachProcess();
        //        //if (CanSwitchToCollect()) SwitchToCollect();
        //        pickerState.Process();
        //        if(pickerState.CanSwitchState()) pickerState = new PickerCollectionState(pickerInfo, this.gameObject, targetResourceObj);
        //        break;
        //    case PickerState.Collect:
        //        pickerState.Process();
        //        if (pickerState.CanSwitchState()) pickerState = new PickerReturnState(pickerInfo, this.gameObject, targetResourceObj,headquartersObj);
        //        break;
        //    case PickerState.Return:
        //        //ReturnProcess();
        //        pickerState.Process();
        //        if (pickerState.CanSwitchState()) pickerState = new PickerApproachState(pickerInfo, this.gameObject, targetResourceObj);
        //        break;
        //}

        pickerState.Process();
        if (pickerState.CanSwitchState())
        {
            Debug.Log($"pickerState.GetType():{pickerState.GetType()}");
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
            state = PickerState.Approach;
        }
        else
        {
            state = PickerState.NotFound;

        }

    }


    private void SearchAgain()
    {
        //最初にターゲットにしていた資源が回収された時に呼び出される

    }

    private void NotFoundProcess()
    {
        //タンクに戻る
    }

    // Approace State
    private void ApproachProcess()
    {
        Debug.Log($"ApproachProcess()");
        var directionVec = Utility.SetYToZero(targetResourceObj.transform.position - transform.position).normalized;
        pickerRd.AddForce(pickerInfo.acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= pickerInfo.maxVelocity) pickerRd.velocity = pickerInfo.maxVelocity * pickerRd.velocity.normalized;
    }

    private bool CanSwitchToCollect()
    {
        var vector = Utility.SetYToZero(targetResourceObj.transform.position - transform.position);
        return vector.magnitude <= collectionRange ;
    }

    private void SwitchToCollect()
    {
        pickerRd.velocity = Vector3.zero;
        collectInitPos = transform.position;
        deltaVector = targetResourceObj.transform.position - collectInitPos;
        state = PickerState.Collect;
    }

    // Collect State
    private void CollectProcess()
    {
        Debug.Log($"CollectProcess()");
        timer += Time.deltaTime;
        if (timer < collectionTime)
        {
            var coefficient = 2 * Mathf.PI / collectionTime;
            var progress = -Mathf.Cos(coefficient * timer) + 1f;
            transform.position = progress * deltaVector + collectInitPos;
        }
        else
        {
            targetResourceObj.transform.position = transform.position - new Vector3(0, collectOffset, 0);
            targetResourceObj.transform.parent = transform;
        }

    }

    private bool CanSwitchToReturn()
    {
       return timer >= collectionTime;
    }

    private void SwitchToReturn()
    {
        state = PickerState.Return;
    }

    // Return State

    private void ReturnProcess()
    {
        Debug.Log($"ReturnProcess()");
        var directionVec = Utility.SetYToZero(headquartersObj.transform.position - transform.position).normalized;
        pickerRd.AddForce(pickerInfo.acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= pickerInfo.maxVelocity) pickerRd.velocity = pickerInfo.maxVelocity * pickerRd.velocity.normalized;
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
    public float acceleration { get; private set; } = 10f;
    public float maxVelocity { get; private set; } = 30;


   public float collectionRange { get; private set; } = 1;
   public float collectionTime { get; private set; } = 2f;
   public float collectOffset { get; private set; } = 1;
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
        Debug.Log($"ApproachProcess()");
        Debug.Log($"targetResourceObj:{targetResourceObj}");
        Debug.Log($"pickerObj:{pickerObj}");

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
            Debug.Log("Collectしました！！");
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
        return false;
    }

}


public interface IPickerState
{
    public abstract void InitProcess();
    public abstract void Process();
    public abstract bool CanSwitchState();
}