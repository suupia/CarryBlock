using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using UnityEditor;
using Vector3 = UnityEngine.Vector3;

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


    public void Init(GameObject playerObj, float rangeRadius)
    {
        Debug.Log($"PickerController.Init() rangeRadius:{rangeRadius}");

        detectionRange = rangeRadius;

        pickerInfo = new PickerInfo(this.gameObject);

        pickerInfo.SetPlayerObj(playerObj);

        headquartersObj = GameObject.Find("Headquarters");
        pickerInfo.SetHeadquartersObj(headquartersObj);

        Search();


        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        pickerState.Process();
        if (pickerState.CanSwitchState())
        {
            if (pickerState.GetType() == typeof(PickerApproachState))
            {
                pickerState = new PickerCollectionState(pickerInfo);
                pickerState.InitProcess();
            }
            else if (pickerState.GetType() == typeof(PickerCollectionState))
            {
                pickerState = new PickerReturnState(pickerInfo);
                pickerState.InitProcess();
            }
            else if (pickerState.GetType() == typeof(PickerReturnState))
            {
                pickerState = new PickerCompleteState(pickerInfo);
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
            pickerState = new PickerApproachState(pickerInfo);
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

    public GameObject pickerObj { get; private set; }
    public Rigidbody pickerRd { get; private set; }
    public GameObject playerObj { get; private set; }
    public GameObject targetResourceObj { get; private set; }
    public GameObject headquartersObj { get; private set; }

    public PickerInfo(GameObject pickerObj)
    {
        this.pickerObj = pickerObj;
        this.pickerRd = pickerObj.GetComponent<Rigidbody>();
    }
    public void SetPlayerObj(GameObject playerObj)
    {
        this.playerObj = playerObj;
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
    protected PickerInfo info;
    GameObject targetResourceObj;
    GameObject headquartersObj;

    protected PickerAbstractState(PickerInfo info)
    {
        this.info = info;
    }

    public abstract void InitProcess();
    public abstract void Process();
    public abstract bool CanSwitchState();

    protected void Move(Vector3 startPos, Vector3 endPos)
    {
        var directionVec = Utility.SetYToZero(endPos - startPos).normalized;
        info.pickerRd.AddForce(info.acceleration * directionVec, ForceMode.Acceleration);
        if (info.pickerRd.velocity.magnitude >= info.maxVelocity) info.pickerRd.velocity = info.maxVelocity * info.pickerRd.velocity.normalized;
    }
}
public class PickerNotFoundState : PickerAbstractState
{
    public PickerNotFoundState(PickerInfo info) : base(info)
    {

    }

    public override void InitProcess()
    {

    }
    public override void Process()
    {
        Move(info.pickerObj.transform.position, info.playerObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        return false;
    }

}

public class PickerApproachState : PickerAbstractState
{
    public PickerApproachState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {

    }
    public override void Process()
    {
        Move(info.pickerObj.transform.position, info.targetResourceObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.targetResourceObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.collectionRange;
    }

}



public class PickerCollectionState : PickerAbstractState
{
    Vector3 initPos;
    Vector3 deltaVector;

    float timer = 0;

    public PickerCollectionState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
        info.pickerRd.velocity = Vector3.zero;
        initPos = info.pickerObj.transform.position;
        deltaVector = info.targetResourceObj.transform.position - initPos;
    }
    public override void Process()
    {
        Debug.Log($"CollectProcess()");
        timer += Time.deltaTime;
        if (timer < info.collectionTime)
        {
            var coefficient = 2 * Mathf.PI / info.collectionTime;
            var progress = -Mathf.Cos(coefficient * timer) + 1f;
            info.pickerObj.transform.position = progress * deltaVector + initPos;
        }
        else
        {
            Debug.Log("complete collect");
            info.targetResourceObj.transform.position = info.pickerObj.transform.position - new Vector3(0, info.collectOffset, 0);
            info.targetResourceObj.transform.parent = info.pickerObj.transform;
        }

    }

    public override bool CanSwitchState()
    {
        return timer >= info.collectionTime;
    }



}

public class PickerReturnState : PickerAbstractState
{

    public PickerReturnState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {

    }
    public override void Process()
    {
        Debug.Log($"ReturnProcess()");
        Move(info.pickerObj.transform.position, info.headquartersObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.headquartersObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnRange;
    }

}

public class PickerCompleteState : PickerAbstractState
{
    public PickerCompleteState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {

    }
    public override void Process()
    {
        Debug.Log($"CompleteProcess()");
        GameObject.Destroy(info.pickerObj);
    }

    public override bool CanSwitchState()
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