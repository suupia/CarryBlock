using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


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
    GameObject targetResourceObj;
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

        pickerContext.CurrentState().Process(pickerContext);
        if (pickerContext.CurrentState().CanSwitchState())
        {
            pickerContext.CurrentState().SwitchState(pickerContext);
            pickerContext.CurrentState().InitProcess();
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

    protected PickerAbstractState(PickerInfo info)
    {
        this.info = info;
    }

    public abstract void InitProcess();
    public abstract void Process(IPickerContext context);
    public abstract bool CanSwitchState();
    public abstract void SwitchState(IPickerContext context);


    protected void Move(Vector3 moveVector,float acceleration, float maxVelocity)
    {
        var directionVec = Utility.SetYToZero(moveVector).normalized;
        info.pickerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;
    }


    //Vector3 prevVelocity = Vector3.zero;
    //Vector3 toEndVector = Vector3.zero;
    //bool isFirstMoveTo = true;
    //Vector3 prevEndPos = Vector3.zero;

    //float restrictionVelocity = float.MaxValue;
    //bool isReaching = false;

    //protected void MoveTo(Vector3 endPos, float acceleration, float maxVelocity)
    //{
    //    var startPos = info.pickerObj.transform.position;
    //    var directionVec = Utility.SetYToZero(endPos - startPos).normalized;
    //    var distance = Utility.SetYToZero(endPos - startPos).magnitude;

    //    if (isFirstMoveTo)
    //    {
    //        isFirstMoveTo = false;
    //        prevVelocity = info.pickerRd.velocity;
    //        prevEndPos = endPos;
    //        toEndVector = endPos - startPos;
    //    }

    //    //EndPos�������Ă��Ȃ��ꍇ
    //    if (Mathf.Approximately((prevEndPos - endPos).magnitude ,0))
    //    {
    //        //�����~�^�������Ȃ��悤�Ɍ���
    //        //�܂葬�x�͒P������
    //        if (distance <= info.decelerationRange && isReaching == false)
    //        {
    //            isReaching = true;
    //            prevEndPos = endPos;
    //            toEndVector = endPos - startPos;
    //        }

    //        if (!isReaching)
    //        {
    //            var deltaVelocity = directionVec; // �{���� deltaVelocity = (directionVec * prevVelocity.magnitude - prevVelocity); �����ǌ덷���o��̂ŋߎ�����
    //            Debug.Log($"deltaVelocity:{deltaVelocity}");
    //            var accelerationDirection = (deltaVelocity / Time.fixedDeltaTime).normalized;
    //            if (accelerationDirection.magnitude < 1.0f) accelerationDirection = directionVec;
    //            Debug.Log($"accelerationDirection:{accelerationDirection}");
    //            var accelerationVector = acceleration * accelerationDirection;
    //            Debug.Log($"accelerationVector:{accelerationVector}");
    //            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
    //            if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;

    //        }
    //        else
    //        {

    //            var preAccelerationVector = (2 / Mathf.Pow(info.estimatedStopTime, 2)) * (toEndVector - prevVelocity * info.estimatedStopTime); //�萔�ł��邱�Ƃɒ���
    //            var accelerationVector = Utility.SetYToZero(preAccelerationVector);
    //            //restrictionVelocity = Mathf.Min(restrictionVelocity,info.pickerRd.velocity.magnitude);
    //            Debug.Log($"accelerationVector:{accelerationVector}");
    //            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
    //        }

    //    }
    //    else
    //    {
    //        //EndPos�������Ă���ꍇ�͓˂�����Œ��n��OK
    //        //�����~�^�������Ȃ��悤�Ɍ���
    //        //if (distance > info.decelerationRange)
    //        //{
    //        //    var deltaVelocity = directionVec; // �{���� deltaVelocity = (directionVec * prevVelocity.magnitude - prevVelocity); �����ǌ덷���o��̂ŋߎ�����
    //        //    Debug.Log($"deltaVelocity:{deltaVelocity}");
    //        //    var accelerationDirection = (deltaVelocity / Time.fixedDeltaTime).normalized;
    //        //    if (accelerationDirection.magnitude < 1.0f) accelerationDirection = directionVec;
    //        //    Debug.Log($"accelerationDirection:{accelerationDirection}");
    //        //    var accelerationVector = acceleration * accelerationDirection;
    //        //    Debug.Log($"accelerationVector:{accelerationVector}");
    //        //    info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
    //        //    if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;

    //        //}
    //        //else
    //        //{

    //        //    //prevVelocity = info.pickerRd.velocity;
    //        //    //toEndVector = endPos - startPos;
    //        //    var preAccelerationVector = (2 / Mathf.Pow(info.estimatedStopTime, 2)) * (toEndVector - prevVelocity * info.estimatedStopTime); //�萔�ł��邱�Ƃɒ���
    //        //    Debug.Log($"(2 / Mathf.Pow(info.stopTime, 2)):{(2 / Mathf.Pow(info.estimatedStopTime, 2))}");
    //        //    Debug.Log($"(toEndVector - prevVector * info.stopTime)):{(toEndVector - prevVelocity * info.estimatedStopTime)}");
    //        //    Debug.Log($"preAccelerationVector:{preAccelerationVector}");

    //        //    var accelerationVector = Utility.SetYToZero(preAccelerationVector);
    //        //    Debug.Log($"accelerationVector:{accelerationVector}");
    //        //    info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
    //        //}
    //    }
    //}

    bool isReach = false;
    bool isFirstReach = true;
    Vector3 prevVelocity;
    Vector3 toEndVector;
    protected void TMoveToFiexedPos(Vector3 endPos, float acceleration, float maxVelocity)
    {
        var pickerPos = Utility.SetYToZero(info.pickerObj.transform.position);
        var pickerVelocity = Utility.SetYToZero( info.pickerRd.velocity);
        var deltaVector = Utility.SetYToZero(endPos - pickerPos); // This vector is assumed to not be zero
        var distance = deltaVector.magnitude;
        if (distance < info.decelerationRange) isReach = true;
        if (!isReach)
        {
            var directionVec = Utility.SetYToZero(deltaVector).normalized;
            var nextVelocity = pickerVelocity.magnitude * directionVec.normalized; // Calculate the acceleration so that the velocity in the next frame aligns with the direction of deltaVector
            var accelerationVector = (nextVelocity - pickerVelocity) / Time.fixedDeltaTime;
            if (accelerationVector.magnitude < 1.0f)
            {
                //Debug.Log($"accelerationVector.magnitude is small");
                accelerationVector = acceleration * deltaVector.normalized;
            }
            else if (accelerationVector.magnitude > acceleration)
            {
                //Debug.Log($"accelerationVector.magnitude is large");
                accelerationVector = acceleration * deltaVector.normalized;
            }
            else
            {
                //Debug.Log($"accelerationVector.magnitude is calculated correctly");
            }
            Debug.Log($"acceleration:{accelerationVector}, accelerationVector QQWr.:{accelerationVector.magnitude}");

            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
            if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;
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
            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration);
        }

    }

    protected void TMoveToMovingPos(Vector3 endPos, float acceleration, float maxVelocity)
    {

    }

    protected void TMoveNormal(Vector3 moveVector){Move(moveVector,info.normalAcceleration,info.normalMaxVelocity);}
    protected void TMoveToFixedPosNormal(Vector3 endPos) { TMoveToFiexedPos(endPos, info.normalAcceleration, info.normalMaxVelocity); }
    protected void TMoveToMovingPosNormal(Vector3 endPos){ TMoveToMovingPos(endPos, info.normalAcceleration, info.normalMaxVelocity); }
    protected void TMoveToFixedPosCarrying(Vector3 endPos) { TMoveToFiexedPos(endPos, info.carryingAcceleration, info.carryingMaxVelocity); }


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
        TMoveNormal(moveVector);

        // search for available resources
        var resource = FindAvailableResource();
        if (resource != null)
        {
            TakeResource(context,resource);
        }
    }

    GameObject FindAvailableResource()
    {
        Collider[] colliders = Physics.OverlapSphere(info.pickerObj.transform.position, info.detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Where(collider => collider.gameObject.GetComponent<ResourceController>().isOwned == false).
            Select(collider => collider.gameObject);

        if (resources.Any())
        {
            return resources.ElementAt(0);
        }
        else
        {
            return null;
        }

    }

    void TakeResource(IPickerContext context, GameObject resource)
    {
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

        TMoveToMovingPosNormal(info.playerObj.transform.position);
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
        if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // �r����null�ɂȂ�\��������
        TMoveToFixedPosNormal(info.targetResourceObj.transform.position);
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

    float timer = 0;

    public PickerCollectState(PickerInfo info) : base(info)
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
        timer += Time.fixedDeltaTime;
        if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState);

        if (timer < info.collectTime)
        {
            var coefficient = 2 * Mathf.PI / info.collectTime;
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
        return timer >= info.collectTime;
    }

    public override void SwitchState(IPickerContext context)
    {
        context.ChangeState(info.returnToMainBaseState);
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
        TMoveToFixedPosCarrying(info.mainBaseObj.transform.position);
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

