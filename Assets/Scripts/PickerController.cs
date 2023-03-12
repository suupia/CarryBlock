using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine.XR;
using Vector3 = UnityEngine.Vector3;


public interface IPickerContext
{
    public void ChangeState(IPickerState state);
}

public interface IPickerState
{
    public abstract void InitProcess();
    public abstract void Process(IPickerContext state);
    public abstract bool CanSwitchState();
    public abstract void SwitchState(IPickerContext state);
}


public class PickerController : MonoBehaviour
{
    bool isInitialized = false;

    // Pure
    private PickerInfo pickerInfo;
    private PickerContext pickerContext;


    // Components
    GameObject targetResourceObj;
    GameObject headquartersObj;
    float detectionRange;


    public void Init(GameObject playerObj, float rangeRadius)
    {
        Debug.Log($"PickerController.Init() rangeRadius:{rangeRadius}");

        detectionRange = rangeRadius;

        pickerInfo = new PickerInfo(this.gameObject,detectionRange);
        pickerContext = new PickerContext();



        pickerInfo.SetPlayerObj(playerObj);

        headquartersObj = GameObject.Find("Headquarters");
        pickerInfo.SetHeadquartersObj(headquartersObj);

        Search();


        isInitialized = true;
    }


    private void Update()
    {
        if (!isInitialized) return;

        pickerContext.currentState.Process(pickerContext);
        if (pickerContext.currentState.CanSwitchState())
        {
            pickerContext.currentState.SwitchState(pickerContext);
            pickerContext.currentState.InitProcess();
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
            pickerContext.ChangeState(pickerInfo.approachState);
            pickerContext.currentState.InitProcess();
        }
        else
        {
            pickerContext.ChangeState(pickerInfo.returnToPlayerState);
            pickerContext.currentState.InitProcess();
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
    // constant fields
    public readonly float acceleration = 10f;
    public readonly float maxVelocity = 30;

    public readonly float collectionRange = 1;
    public readonly float collectionTime = 1.5f;
    public readonly float collectOffset = 1;

    public readonly float returnToHeadquartersRange = 1;
    public readonly float returnToPlayerRange = 2;

    // singletons
    public IPickerState approachState { get; private set; }
    public IPickerState collectionState { get; private set; }
    public IPickerState returnToHeadquartersState { get; private set; }
    public IPickerState returnToPlayerState { get; private set; }
    public IPickerState completeState { get; private set; }

    // components
    public GameObject pickerObj { get; private set; }
    public Rigidbody pickerRd { get; private set; }
    public GameObject playerObj { get; private set; }
    public GameObject targetResourceObj { get; private set; }
    public GameObject headquartersObj { get; private set; }

    // injected fields

    public  float detectionRange { get; private set; }

    public PickerInfo(GameObject pickerObj, float detectionRange)
    {
        this.pickerObj = pickerObj;
        this.pickerRd = pickerObj.GetComponent<Rigidbody>();

        approachState = new PickerApproachState(this);
        collectionState = new PickerCollectionState(this);
        returnToHeadquartersState = new PickerReturnToHeadquartersState(this);
        returnToPlayerState = new PickerReturnToPlayerState(this);
        completeState = new PickerCompleteState(this);

        this.detectionRange = detectionRange;
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



public class PickerContext : IPickerContext
{
    public IPickerState currentState { get; private set; }


    public void ChangeState(IPickerState state)
    {
        this.currentState = state;
    }
}
public abstract class PickerAbstractState : IPickerState
{
    protected PickerInfo info;

    protected PickerAbstractState(PickerInfo info)
    {
        this.info = info;
    }


    public abstract void InitProcess();
    public abstract void Process(IPickerContext context);
    public abstract bool CanSwitchState();
    public abstract void SwitchState(IPickerContext context);
    protected void Move(Vector3 startPos, Vector3 endPos)
    {
        var directionVec = Utility.SetYToZero(endPos - startPos).normalized;
        info.pickerRd.AddForce(info.acceleration * directionVec, ForceMode.Acceleration);
        if (info.pickerRd.velocity.magnitude >= info.maxVelocity) info.pickerRd.velocity = info.maxVelocity * info.pickerRd.velocity.normalized;
    }

    protected void Move(Vector3 moveVector)
    {
        var directionVec = Utility.SetYToZero(moveVector).normalized;
        info.pickerRd.AddForce(info.acceleration * directionVec, ForceMode.Acceleration);
        if (info.pickerRd.velocity.magnitude >= info.maxVelocity) info.pickerRd.velocity = info.maxVelocity * info.pickerRd.velocity.normalized;
    }
}

public class PickerSearchState : PickerAbstractState
{
    private float timer = 0;
    private readonly float minSpawnTime = 1;
    public PickerSearchState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
        timer = 0;
    }
    public override void Process(IPickerContext context)
    {
        timer += Time.deltaTime;
        var moveVector = info.pickerObj.transform.forward;
        Move(moveVector);

        //‚à‚µresource‚ðŒ©‚Â‚¯‚½‚çó‘Ô‚ð•ÏX
        Collider[] colliders = Physics.OverlapSphere(info.pickerObj.transform.position, info.detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Select(collider => collider.gameObject);
        if (resources.Any())
        {
            info.SetTargetResourceObj(resources.ElementAt(0));
            context.ChangeState(info.approachState);
        }

    }

    public override bool CanSwitchState()
    {
        return timer > minSpawnTime;
    }

    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.returnToPlayerState);
    }

}

public class PickerReturnToPlayerState : PickerAbstractState
{
    private float timer = 0;
    private readonly float minSpawnTime = 1;

    public PickerReturnToPlayerState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
        timer = 0;
    }
    public override void Process(IPickerContext context)
    {
        timer += Time.deltaTime;
        Move(info.pickerObj.transform.position, info.playerObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.playerObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToPlayerRange && timer > minSpawnTime;
    }

    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.completeState);
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
    public override void Process(IPickerContext context)
    {
        Move(info.pickerObj.transform.position, info.targetResourceObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.targetResourceObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.collectionRange;
    }
    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.collectionState);
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
    public override void Process(IPickerContext context)
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

    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.returnToHeadquartersState);
    }


}

public class PickerReturnToHeadquartersState : PickerAbstractState
{

    public PickerReturnToHeadquartersState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {

    }
    public override void Process(IPickerContext context)
    {
        Debug.Log($"ReturnProcess()");
        Move(info.pickerObj.transform.position, info.headquartersObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.headquartersObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToHeadquartersRange;
    }
    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.completeState);
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
    public override void Process(IPickerContext context)
    {
        Debug.Log($"CompleteProcess()");
        Object.Destroy(info.pickerObj);
    }

    public override bool CanSwitchState()
    {
        return false;
    }
    public override void SwitchState(IPickerContext context)
    {
        Debug.LogWarning($"PickerCompleteState.SwitchState(context:{context})");
    }

}

