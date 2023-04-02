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
    public void Process(IPickerContext state);
}


public class PickerController : MonoBehaviour
{
    bool isInitialized = false;

    // Pure
    PickerInfo pickerInfo;
    PlayerInfoWrapper playerInfoWrapper;
    IPickerContext pickerContext;


    // Components
    GameObject mainBaseObj;


    public void Init(GameObject playerObj, PlayerInfoWrapper infoWrapper)
    {
        this.playerInfoWrapper = infoWrapper;
        Debug.Log($"infoWrapper.RangeRadius:{infoWrapper.RangeRadius}");

        pickerInfo = new PickerInfo(this.gameObject, infoWrapper);
        pickerInfo.SetPlayerObj(playerObj);
        mainBaseObj = GameObject.Find("MainBase");
        pickerInfo.SetMainBaseObj(mainBaseObj);

        pickerContext = new PickerContext(pickerInfo.SearchState);


        isInitialized = true;
    }


    private void FixedUpdate()
    {
        if (!isInitialized) return;

        pickerContext.CurrentState().Process(pickerContext);

    }

    // Debug
    void OnDrawGizmos()
    {
        var detectionRange = playerInfoWrapper.RangeRadius;
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
    public IPickerState SearchState => new PickerSearchState(this);
    public IPickerState ApproachState => new PickerApproachState(this);
    public IPickerState CollectState => new PickerCollectState(this);
    public IPickerState ReturnToMainBaseState => new PickerReturnToMainBaseState(this);
    public IPickerState ReturnToPlayerState => new PickerReturnToPlayerState(this);
    public IPickerState CompleteState => new PickerCompleteState(this);

    // components
    public GameObject pickerObj { get; private set; }
    public Rigidbody pickerRd { get; private set; }
    public GameObject playerObj { get; private set; }
    public Rigidbody playerRd { get; private set; }
    public GameObject targetResourceObj { get; private set; }
    public GameObject mainBaseObj { get; private set; }


    // injected fields
    public float detectionRange { get; private set; }

    public PickerInfo(GameObject pickerObj, PlayerInfoWrapper infoWrapper)
    {
        this.pickerObj = pickerObj;
        this.pickerRd = pickerObj.GetComponent<Rigidbody>();

        this.detectionRange = infoWrapper.RangeRadius;
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

    public abstract void Process(IPickerContext context);

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
        ForwardNormal, ToFixedPosNormal, ToMovingPosNormal, ToFixedPosCarrying
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
        if (state != MoveState.ToFixedPosNormal) Reset(MoveState.ToFixedPosNormal, endPos);
        MoveToFixedPos(endPos, info.normalAcceleration, info.normalMaxVelocity);
    }

    public void MoveToMovingPosNormal(Vector3 endPos)
    {
        if (state != MoveState.ToMovingPosNormal) Reset(MoveState.ToMovingPosNormal, endPos);
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

    public override void Process(IPickerContext context)
    {
        timer += Time.fixedDeltaTime;


        if (CanSwitchState())
        {
            context.ChangeState(info.ReturnToPlayerState);
        }
        else
        {
            // move in the direction the player is facing
            var moveVector = info.playerObj.transform.forward;
            mover.MoveForwardNormal(moveVector);

            // try to take available resource
            AttemptTakeResource(context);
        }


    }
    bool CanSwitchState()
    {
        return timer > minSpawnTime;
    }

    void AttemptTakeResource(IPickerContext context)
    {
        Collider[] colliders = Physics.OverlapSphere(Utility.SetYToZero(info.pickerObj.transform.position), info.detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Where(collider => collider.gameObject.GetComponent<ResourceController>().isOwned == false).
            Select(collider => collider.gameObject);

        if (resources.Any()) TakeResource(context, resources.First());

    }

    void TakeResource(IPickerContext context, GameObject resource)
    {
        if (resource == null) return;
        resource.GetComponent<ResourceController>().isOwned = true;
        info.SetTargetResourceObj(resource);
        context.ChangeState(info.ApproachState);
    }



}

public class PickerReturnToPlayerState : PickerAbstractState
{
    public PickerReturnToPlayerState(PickerInfo info) : base(info)
    {
    }

    public override void Process(IPickerContext context)
    {
        if (info.targetResourceObj == null) context.ChangeState(info.ReturnToPlayerState);

        if (CanSwitchState())
        {
            context.ChangeState(info.CompleteState);
        }
        else
        {
            mover.MoveToMovingPosNormal(info.playerObj.transform.position);
        }



    }

    bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.playerObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToPlayerRange;
    }
}

public class PickerApproachState : PickerAbstractState
{
    public PickerApproachState(PickerInfo info) : base(info)
    {
    }

    public override void Process(IPickerContext context)
    {
        if (CanSwitchState())
        {
            context.ChangeState(info.CollectState);
        }
        else
        {
            //if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // A flag has been added, so there is no longer a possibility of null during execution
            mover.MoveToFixedPosNormal(info.targetResourceObj.transform.position);
        }
    }

    bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.targetResourceObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.collectRange;
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
        initPos = info.pickerObj.transform.position;
        deltaVector = info.targetResourceObj.transform.position - initPos;
    }


    public override void Process(IPickerContext context)
    {
        // Debug.Log($"CollectProcess()");
        if (CanSwitchState())
        {
            context.ChangeState(info.ReturnToMainBaseState);
        }
        else
        {
            CollectResource(context);
        }

    }

    bool CanSwitchState()
    {
        return isComplete;
    }

    async void CollectResource(IPickerContext context)
    {
        if (isCollecting) return;
        isCollecting = true;

        for (float t = 0; t < info.collectTime; t += Time.deltaTime)
        {
            // if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // A flag has been added, so there is no longer a possibility of null during execution

            var coefficient = 2 * Mathf.PI / info.collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;
            info.pickerObj.transform.position = progress * deltaVector + initPos;

            await UniTask.Yield();
        }

        info.targetResourceObj.transform.position = info.pickerObj.transform.position - new Vector3(0, info.collectOffset, 0);
        info.targetResourceObj.transform.parent = info.pickerObj.transform;

        isCollecting = false;
        isComplete = true;
    }

}

public class PickerReturnToMainBaseState : PickerAbstractState
{

    public PickerReturnToMainBaseState(PickerInfo info) : base(info)
    {
    }

    public override void Process(IPickerContext context)
    {
        //Debug.Log($"ReturnProcess()");

        if (CanSwitchState())
        {
            context.ChangeState(info.CompleteState);
        }
        else
        {
            mover.MoveToFixedPosCarrying(info.mainBaseObj.transform.position);

        }


    }

    bool CanSwitchState()
    {
        var vector = Utility.SetYToZero(info.mainBaseObj.transform.position - info.pickerObj.transform.position);
        return vector.magnitude <= info.returnToMainBaseRange;
    }

}



public class PickerCompleteState : PickerAbstractState
{
    public PickerCompleteState(PickerInfo info) : base(info)
    {
    }

    public override void Process(IPickerContext context)
    {
        //Debug.Log($"CompleteProcess()");
        Object.Destroy(info.pickerObj);


    }

}

