using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickerController : MonoBehaviour
{

    float acceleration = 10f;
    float maxVelocity = 30;


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
    }

    private void Update()
    {
        switch (state)
        {
            case PickerState.NotFound:

                break;
            case PickerState.Approach:
                ApproachProcess();
                if (CanSwitchToCollect()) SwitchToCollect();
                break;
            case PickerState.Collect:
                CollectProcess();
                if(CanSwitchToReturn()) SwitchToReturn();
                break;
            case PickerState.Return:
                ReturnProcess();
                break;
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
        pickerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= maxVelocity) pickerRd.velocity = maxVelocity * pickerRd.velocity.normalized;
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
        pickerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= maxVelocity) pickerRd.velocity = maxVelocity * pickerRd.velocity.normalized;
    }

    // Debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}


// State Pattern

//public class PickerApproachState : PickerState
//{
//    GameObject pickerObj;
//    Rigidbody pickerRd;
//    GameObject targetResourceObj;
//    public PickerApproachState(GameObject pickerObj, GameObject targetResourceObj)
//    {
//        this.pickerObj = pickerObj;
//        pickerRd = pickerObj.GetComponent<Rigidbody>();
//        this.targetResourceObj = targetResourceObj;
//    }
//    public void Process()
//    {
//        //資源に向かう
//        var directionVec = Utility.SetYToZero(targetResourceObj.transform.position - pickerObj.transform.position).normalized;
//        pickerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
//        if (pickerRd.velocity.magnitude >= maxVelocity) pickerRd.velocity = maxVelocity * pickerRd.velocity.normalized;

//    }
//}

//public interface PickerState
//{
//    public abstract void Process();
//    public abstract bool CanSwitchState();
//    public abstract void OnSwitchState();
//}