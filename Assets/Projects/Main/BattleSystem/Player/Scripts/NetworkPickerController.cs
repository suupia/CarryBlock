using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Main
{
    public interface IPickerContext
    {
        public IPickerState CurrentState();
        public void ChangeState(IPickerState state);
    }

    public interface IPickerState
    {
        public void Process(IPickerContext state);
    }

    public class NetworkPickerController : PoolableObject
    {
        // Pure
        [SerializeField] PickerInfo pickerInfo;


        // Components
        List<GameObject> baseObjs = new();
        bool isInitialized;
        IPickerContext pickerContext;

        private void FixedUpdate()
        {
            if (!isInitialized) return;

            pickerContext.CurrentState().Process(pickerContext);
        }


        public void Init(NetworkRunner runner, GameObject playerObj, PlayerInfoForPicker info)
        {
            Debug.Log($"infoWrapper.RangeRadius:{info.RangeRadius}");

            pickerInfo = new PickerInfo(runner, Object, info);
            pickerInfo.SetPlayerObj(playerObj);
            baseObjs.Add(GameObject.FindWithTag("MainBase"));
            baseObjs = baseObjs.Concat(GameObject.FindGameObjectsWithTag("SubBase")).ToList();
            pickerInfo.SetMainBaseObj(baseObjs);

            pickerContext = new PickerContext(pickerInfo.SearchState);


            isInitialized = true;
        }

        protected override void OnInactive()
        {
            if (!isInitialized) return;
            pickerInfo.pickerRd.velocity = Vector3.zero;
        }

        // Debug
        // void OnDrawGizmos()
        // {
        //     var detectionRange = playerInfoWrapper.RangeRadius;
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawWireSphere(transform.position, detectionRange);
        // }
    }

    // State Pattern

    [Serializable]
    public class PickerInfo
    {
        public readonly float carryingAcceleration = 150f;
        public readonly float carryingMaxVelocity = 8;
        public readonly float collectOffset = 1;


        public readonly float collectRange = 2.0f;
        public readonly float collectTime = 1.2f;
        public readonly float decelerationRange = 6;
        public readonly float estimatedStopTime = 0.3f; // Estimated time to decelerate to a stop

        // constant fields
        public readonly float normalAcceleration = 300f;
        public readonly float normalMaxVelocity = 25;

        public readonly float returnToMainBaseRange = 1;

        public readonly float returnToPlayerRange = 2.5f;

        // runner
        public readonly NetworkRunner runner;

        public PickerInfo(NetworkRunner runner, NetworkObject pickerObj, PlayerInfoForPicker info)
        {
            this.runner = runner;
            this.pickerObj = pickerObj;
            Debug.Log($"pickerObj = {pickerObj}");
            pickerRd = pickerObj.GetComponent<Rigidbody>();

            detectionRange = info.RangeRadius;
        }

        // singletons
        public IPickerState SearchState => new PickerSearchState(this);
        public IPickerState ApproachState => new PickerApproachState(this);
        public IPickerState CollectState => new PickerCollectState(this);
        public IPickerState ReturnToMainBaseState => new PickerReturnToBaseState(this);
        public IPickerState ReturnToPlayerState => new PickerReturnToPlayerState(this);
        public IPickerState CompleteState => new PickerCompleteState(this);

        // components
        public NetworkObject pickerObj { get; private set; }
        public Rigidbody pickerRd { get; private set; }
        public GameObject playerObj { get; private set; }
        public Rigidbody playerRd { get; private set; }
        public NetworkObject targetResourceObj { get; private set; }
        public IEnumerable<GameObject> baseObjs { get; private set; }


        // injected fields
        public float detectionRange { get; private set; }

        public void SetPlayerObj(GameObject playerObj)
        {
            this.playerObj = playerObj;
            playerRd = playerObj.gameObject.GetComponent<Rigidbody>();
        }


        public void SetTargetResourceObj(NetworkObject targetResourceObj)
        {
            this.targetResourceObj = targetResourceObj;
        }

        public void SetMainBaseObj(IEnumerable<GameObject> mainBaseObj)
        {
            baseObjs = mainBaseObj;
        }
    }

#nullable enable
    public class PickerContext : IPickerContext
    {
        public PickerContext(IPickerState initState)
        {
            currentState = initState;
        }

        public IPickerState currentState { get; private set; }

        public IPickerState CurrentState()
        {
            return currentState;
        }

        public void ChangeState(IPickerState state)
        {
            currentState = state;
        }
    }

    public abstract class PickerAbstractState : IPickerState
    {
        protected PickerInfo info;
        protected PickerMover mover;

        protected PickerAbstractState(PickerInfo info)
        {
            this.info = info;
            mover = new PickerMover(info);
        }

        public abstract void Process(IPickerContext context);
    }

    public class PickerMover
    {
        readonly PickerInfo info;

        // These fields are used in MoveToFixedPos()
        Vector3 initDeltaVector;
        bool isFirstReach = true;
        bool isPast;
        bool isReach;
        Vector3 prevVelocity;
        MoveState state;
        Vector3 toEndVector;

        public PickerMover(PickerInfo info)
        {
            this.info = info;
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
            var nextVelocity =
                maxVelocity *
                directionVec; // Calculate the acceleration so that the velocity in the next frame aligns with the direction of deltaVector
            var accelerationVector = (nextVelocity - pickerVelocity) / Time.fixedDeltaTime;
            if (accelerationVector.magnitude > acceleration)
            {
                //Debug.Log($"accelerationVector.magnitude is large");
                accelerationVector = acceleration * deltaVector.normalized;
            }

            //Debug.Log($"accelerationVector.magnitude is calculated correctly");
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

                var preAccelerationVector = 2 / Mathf.Pow(info.estimatedStopTime, 2) *
                                            (toEndVector -
                                             prevVelocity * info.estimatedStopTime); // Note that it is a constant.
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
            if (info.pickerRd.velocity.magnitude >= maxVelocity)
                info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;
        }

        enum MoveState
        {
            ForwardNormal,
            ToFixedPosNormal,
            ToMovingPosNormal,
            ToFixedPosCarrying
        }
    }

    public class PickerSearchState : PickerAbstractState
    {
        readonly float minSpawnTime = 0.3f;
        float timer;

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
            Collider[] colliders = Physics.OverlapSphere(Utility.SetYToZero(info.pickerObj.transform.position),
                info.detectionRange);
            var resources = colliders.Where(collider => collider.CompareTag("Resource"))
                .Where(collider => collider.gameObject.GetComponent<NetworkResourceController>().canAccess)
                .Select(collider => collider.gameObject.GetComponent<NetworkObject>());

            if (resources.Any()) TakeResource(context, resources.First());
        }

        void TakeResource(IPickerContext context, NetworkObject resource)
        {
            if (resource == null) return;
            resource.GetComponent<NetworkResourceController>().OnReserved();
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
                context.ChangeState(info.CompleteState);
            else
                mover.MoveToMovingPosNormal(info.playerObj.transform.position);
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
                context.ChangeState(info.CollectState);
            else
                //if (info.targetResourceObj == null) context.ChangeState(info.returnToPlayerState); // A flag has been added, so there is no longer a possibility of null during execution
                mover.MoveToFixedPosNormal(info.targetResourceObj.transform.position);
        }

        bool CanSwitchState()
        {
            var vector =
                Utility.SetYToZero(info.targetResourceObj.transform.position - info.pickerObj.transform.position);
            return vector.magnitude <= info.collectRange;
        }
    }

    public class PickerCollectState : PickerAbstractState
    {
        readonly Vector3 deltaVector;
        readonly Vector3 initPos;

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
                context.ChangeState(info.ReturnToMainBaseState);
            else
                CollectResource(context);
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

            info.targetResourceObj.transform.position =
                info.pickerObj.transform.position - new Vector3(0, info.collectOffset, 0);
            // info.targetResourceObj.transform.parent = info.pickerObj.transform;
            info.targetResourceObj.GetComponent<NetworkResourceController>().OnHeld(info.pickerObj.transform);

            isCollecting = false;
            isComplete = true;
        }
    }

    public class PickerReturnToBaseState : PickerAbstractState
    {
        public PickerReturnToBaseState(PickerInfo info) : base(info)
        {
        }

        public override void Process(IPickerContext context)
        {
            //Debug.Log($"ReturnProcess()");

            if (CanSwitchState())
                context.ChangeState(info.CompleteState);
            else
                mover.MoveToFixedPosCarrying(GetNearestBasePos());
        }

        Vector3 GetNearestBasePos()
        {
            var resultPos = Vector3.zero;
            var minDistance = float.MaxValue;
            var pos = info.pickerObj.transform.position;
            foreach (var baseObj in info.baseObjs)
            {
                var distance = Vector3.Distance(pos, baseObj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    resultPos = baseObj.transform.position;
                }
            }

            if (resultPos == Vector3.zero) Debug.LogError("GetNearestBasePos() was failed");
            return resultPos;
        }

        bool CanSwitchState()
        {
            var vector = Utility.SetYToZero(GetNearestBasePos() - info.pickerObj.transform.position);
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
            Debug.Log("Delete Picker");
            info.runner.Despawn(info.targetResourceObj);
            info.runner.Despawn(info.pickerObj);
        }
    }
}