using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Cysharp.Threading.Tasks;


public interface IPickerContext
{
    public IPickerState CurrentState();
    public void ChangeState(IPickerState state);
}

public interface IPickerState
{
    public void InitProcess();
    public void Process(IPickerContext state);
    public bool CanSwitchState();
    public void SwitchState(IPickerContext state);
}


public class PickerController : MonoBehaviour
{
    bool isInitialized = false;

    // Pure
    PickerInfo pickerInfo;
    IPickerContext pickerContext;


    // Components
    GameObject mainBaseObj;
    float detectionRange;


    public void Init(GameObject playerObj, float rangeRadius)
    {
        Debug.Log($"PickerController.Init() rangeRadius:{rangeRadius}");

        detectionRange = rangeRadius;

        pickerInfo = new PickerInfo(this.gameObject, detectionRange);
        pickerContext = new PickerContext(pickerInfo.searchState);

        pickerInfo.SetPlayerObj(playerObj);

        mainBaseObj = GameObject.Find("MainBase");
        pickerInfo.SetMainBaseObj(mainBaseObj);


        isInitialized = true;
    }


    private void FixedUpdate()
    {
        if (!isInitialized) return;

        if (pickerContext.CurrentState().CanSwitchState()) // Check if the current state of the picker can switch to another state before Process().
        {
            pickerContext.CurrentState().SwitchState(pickerContext);
            pickerContext.CurrentState().InitProcess();
        }
        pickerContext.CurrentState().Process(pickerContext);

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
    public readonly float normalAcceleration = 300f;
    public readonly float carryingAcceleration = 150f;
    public readonly float normalMaxVelocity = 50;
    public readonly float carryingMaxVelocity = 25;
    public readonly float decelerationRange = 6;
    public readonly float estimatedStopTime = 0.3f; // Estimated time to decelerate to a stop


    public readonly float collectRange = 2.0f;
    public readonly float collectTime = 1.2f;
    public readonly float collectOffset = 1;

    public readonly float returnToMainBaseRange = 1;
    public readonly float returnToPlayerRange = 2.5f;

    // singletons
    public IPickerState searchState { get; private set; }
    public IPickerState approachState { get; private set; }
    public IPickerState collectState { get; private set; }
    public IPickerState returnToMainBaseState { get; private set; }
    public IPickerState returnToPlayerState { get; private set; }
    public IPickerState completeState { get; private set; }

    // components
    public GameObject pickerObj { get; private set; }
    public Rigidbody pickerRd { get; private set; }
    public GameObject playerObj { get; private set; }
    public Rigidbody playerRd { get; private set; }
    public GameObject targetResourceObj { get; private set; }
    public GameObject mainBaseObj { get; private set; }


    // injected fields
    public float detectionRange { get; private set; }

    public PickerInfo(GameObject pickerObj, float detectionRange)
    {
        this.pickerObj = pickerObj;
        this.pickerRd = pickerObj.GetComponent<Rigidbody>();

        searchState = new PickerSearchState(this);
        approachState = new PickerApproachState(this);
        collectState = new PickerCollectState(this);
        returnToMainBaseState = new PickerReturnToMainBaseState(this);
        returnToPlayerState = new PickerReturnToPlayerState(this);
        completeState = new PickerCompleteState(this);

        this.detectionRange = detectionRange;
    }
    public void SetPlayerObj(GameObject playerObj)
    {
        this.playerObj = playerObj;
        this.playerRd = playerObj.gameObject.GetComponent<Rigidbody>();
    }


    public void SetTargetResourceObj(GameObject targetResourceObj)
    {
        this.targetResourceObj = targetResourceObj;
    }

    public void SetMainBaseObj(GameObject mainBaseObj)
    {
        this.mainBaseObj = mainBaseObj;
    }
}

#nullable enable
public class PickerContext : IPickerContext
{
    public IPickerState currentState { get; private set; }

    public PickerContext(IPickerState initState)
    {
        this.currentState = initState;
    }

    public IPickerState CurrentState()
    {
        return currentState;
    }

    public void ChangeState(IPickerState state)
    {
        this.currentState = state;
    }
}
public abstract class PickerAbstractState : IPickerState
{
    protected PickerInfo info;
    protected PickerMover mover;

    protected PickerAbstractState(PickerInfo info)
    {
        this.info = info;
        this.mover = new PickerMover(info);
    }

    public abstract void InitProcess();
    public abstract void Process(IPickerContext context);
    public abstract bool CanSwitchState();
    public abstract void SwitchState(IPickerContext context);

}

public class PickerMover
{
    PickerInfo info;
    MoveState state;

    // These fields are used in MoveToFixedPos()
    Vector3 initDeltaVector;
    bool isPast = false;
    bool isReach = false;
    bool isFirstReach = true;
    Vector3 prevVelocity;
    Vector3 toEndVector;

    public PickerMover(PickerInfo info)
    {
        this.info = info;
    }

    enum MoveState
    {
        ForwardNormal,ToFixedPosNormal,ToMovingPosNormal,ToFixedPosCarrying
    }

    public void MoveForwardNormal(Vector3 moveVector)
    {
        if (state != MoveState.ForwardNormal)
        {
            var dummyEndPos = Utility.SetYToZero(info.pickerObj.transform.position) + moveVector;
            Reset(MoveState.ForwardNormal, dummyEndPos);
        }
        Move(moveVector, info.normalAcceleration, info.normalMaxVelocity);
    }

    public void MoveToFixedPosNormal(Vector3 endPos)
    {
        if (state != MoveState.ToFixedPosNormal) Reset(MoveState.ToFixedPosNormal,endPos);
        MoveToFixedPos(endPos, info.normalAcceleration, info.normalMaxVelocity);
    }

    public void MoveToMovingPosNormal(Vector3 endPos)
    {
        if (state != MoveState.ToMovingPosNormal) Reset(MoveState.ToMovingPosNormal,endPos);
        MoveToMovingPos(endPos, info.normalAcceleration, info.normalMaxVelocity);
    }

    public void MoveToFixedPosCarrying(Vector3 endPos)
    {
        if (state != MoveState.ToFixedPosCarrying) Reset(MoveState.ToFixedPosCarrying, endPos);
        MoveToFixedPos(endPos, info.carryingAcceleration, info.carryingMaxVelocity);
    }

    void Reset(MoveState state, Vector3 endPos)
    {
        this.state = state;
        var pickerPos = Utility.SetYToZero(info.pickerObj.transform.position);
        initDeltaVector = Utility.SetYToZero(endPos - pickerPos); // This vector is assumed to not be zero
    }

    void Move(Vector3 moveVector, float acceleration, float maxVelocity)
    {
        var directionVec = Utility.SetYToZero(moveVector).normalized;
        AddForceByLimitVelocity(acceleration * directionVec, maxVelocity);
    }

    void AccelerateMove(Vector3 endPos, float acceleration, float maxVelocity)
    {
        var pickerPos = Utility.SetYToZero(info.pickerObj.transform.position);
        var pickerVelocity = Utility.SetYToZero(info.pickerRd.velocity);

        var deltaVector = Utility.SetYToZero(endPos - pickerPos); // This vector is assumed to not be zero

        var directionVec = Utility.SetYToZero(deltaVector).normalized;
        var nextVelocity = maxVelocity * directionVec; // Calculate the acceleration so that the velocity in the next frame aligns with the direction of deltaVector
        var accelerationVector = (nextVelocity - pickerVelocity) / Time.fixedDeltaTime;
        if (accelerationVector.magnitude > acceleration)
        {
            //Debug.Log($"accelerationVector.magnitude is large");
            accelerationVector = acceleration * deltaVector.normalized;
        }
        else
        {
            //Debug.Log($"accelerationVector.magnitude is calculated correctly");
        }
        //Debug.Log($"accelerationVector:{accelerationVector}, accelerationVector.magnitude:{accelerationVector.magnitude}");
        AddForceByLimitVelocity(accelerationVector, maxVelocity);
    }


    void MoveToFixedPos(Vector3 endPos, float acceleration, float maxVelocity)
    {
        var pickerPos = Utility.SetYToZero(info.pickerObj.transform.position);
        var pickerVelocity = Utility.SetYToZero(info.pickerRd.velocity);
        var deltaVector = Utility.SetYToZero(endPos - pickerPos); // This vector is assumed to not be zero
        var distance = deltaVector.magnitude;

        if (Mathf.Abs(Vector3.Angle(initDeltaVector, deltaVector)) >= 90) isPast = true;
        if (distance < info.decelerationRange) isReach = true;
        if (!isReach && !isPast)
        {
            AccelerateMove(endPos, acceleration, maxVelocity);
        }
        else
        {
            //Debug.Log($"going into landing gear");
            if (isFirstReach)
            {
                isFirstReach = false;
                prevVelocity = info.pickerRd.velocity;
                toEndVector = endPos - pickerPos;
            }
            var preAccelerationVector = (2 / Mathf.Pow(info.estimatedStopTime, 2)) * (toEndVector - prevVelocity * info.estimatedStopTime); // Note that it is a constant.
            var accelerationVector = Utility.SetYToZero(preAccelerationVector);
            //Debug.Log($"accelerationVector:{accelerationVector}, accelerationVector.magnitude:{accelerationVector.magnitude}");
            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
        }
    }

    void MoveToMovingPos(Vector3 endPos, float acceleration, float maxVelocity)
    {
        AccelerateMove(endPos, acceleration, maxVelocity);
    }
    void AddForceByLimitVelocity(Vector3 accelerationVector, float maxVelocity)
    {
        info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
        if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;
    }

}

public class PickerSearchState : PickerAbstractState
{
    float timer = 0;
    readonly float minSpawnTime = 0.3f;
    public PickerSearchState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
        timer = 0;
    }
    public override void Process(IPickerContext context)
    {
        timer += Time.fixedDeltaTime;

        // move in the direction the player is facing
        var moveVector = info.playerObj.transform.forward;
        mover.MoveForwardNormal(moveVector);

        // try to take available resource
        AttemptTakeResource(context);
    }

    void AttemptTakeResource(IPickerContext context)
    {
        Collider[] colliders = Physics.OverlapSphere(Utility.SetYToZero(info.pickerObj.transform.position), info.detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Where(collider => collider.gameObject.GetComponent<ResourceController>().isOwned == false).
            Select(collider => collider.gameObject);

        if( resources.Any())TakeResource(context, resources.First());

    }

    void TakeResource(IPickerContext context, GameObject resource)
    {
        if(resource == null) return;
        resource.GetComponent<ResourceController>().isOwned = true;
        info.SetTargetResourceObj(resource);
        context.ChangeState(info.approachState);
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
    public PickerReturnToPlayerState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
    }
    public override void Process(IPickerContext context)
    {
        if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState);

        mover.MoveToMovingPosNormal(info.playerObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.playerObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToPlayerRange;
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
        //if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // A flag has been added, so there is no longer a possibility of null during execution
        mover.MoveToFixedPosNormal(info.targetResourceObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.targetResourceObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.collectRange;
    }
    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.collectState);
    }

}



public class PickerCollectState : PickerAbstractState
{
    Vector3 initPos;
    Vector3 deltaVector;

    bool isCollecting;
    bool isComplete;

    public PickerCollectState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {
        //info.pickerRd.velocity = Vector3.zero;
        initPos = info.pickerObj.transform.position;
        deltaVector = info.targetResourceObj.transform.position - initPos;
    }
    public override void Process(IPickerContext context)
    {
        Debug.Log($"CollectProcess()");
        CollectResource(context);

    }

    public override bool CanSwitchState()
    {
        return isComplete;
    }

    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.returnToMainBaseState);
    }

    async void CollectResource(IPickerContext context)
    {
        if(isCollecting)return;

        isCollecting = true;

        for (float t = 0; t < info.collectTime; t += Time.deltaTime)
        {
           // if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // A flag has been added, so there is no longer a possibility of null during execution

            var coefficient = 2 * Mathf.PI / info.collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;
            info.pickerObj.transform.position = progress * deltaVector + initPos;

            await UniTask.Yield();
        }

        Debug.Log("complete collect");
        info.targetResourceObj.transform.position = info.pickerObj.transform.position - new Vector3(0, info.collectOffset, 0);
        info.targetResourceObj.transform.parent = info.pickerObj.transform;
        isComplete = true;

        isCollecting = false;
    }

}

public class PickerReturnToMainBaseState : PickerAbstractState
{

    public PickerReturnToMainBaseState(PickerInfo info) : base(info)
    {
    }

    public override void InitProcess()
    {

    }
    public override void Process(IPickerContext context)
    {
        Debug.Log($"ReturnProcess()");
        mover.MoveToFixedPosCarrying(info.mainBaseObj.transform.position);
    }

    public override bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.mainBaseObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToMainBaseRange;
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

