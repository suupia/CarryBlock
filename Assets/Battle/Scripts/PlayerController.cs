using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject rangeCirclePrefab;

    [SerializeField] GameObject[] unitPrefabs;

    [SerializeField] UnitType unitType;

    [SerializeField] PlayerInfo info;

    PlayerUnit playerUnit;
    PlayerShooter shooter;

    public enum UnitType
    {
        Tank, Plane,
    }

    void Start()
    {
        var playerObj = Instantiate(unitPrefabs[(int)unitType],gameObject.transform);
        var playerCamera  = Instantiate(cameraPrefab, playerObj.transform);
        var rangeCircleObj = Instantiate(rangeCirclePrefab, playerObj.transform);

        info.Init(playerObj);
        playerUnit = unitType switch
        {
            UnitType.Tank => new PlayerTank(info),
            UnitType.Plane => new PlayerPlane(info),
            _ => throw new ArgumentOutOfRangeException(nameof(unitType), "Invalid unitType")
        };
        shooter = new PlayerShooter(info);
    }

    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        var direction = Vector3.Normalize(new Vector3(horizontalInput, 0, verticalInput));

        playerUnit.MoveUnit(direction);

        if (Input.GetMouseButtonDown(0))
        {
            playerUnit.UnitAction();
        }

        var targetEnemy = shooter.FindEnemy();
        shooter.ShootEnemy(targetEnemy);
    }
}

[Serializable]
public class PlayerInfo
{
    // constant fields
    public readonly float acceleration = 10f;
    public readonly float maxVelocity = 15;
    public readonly float bulletOffset = 1;
    public readonly float resistance = 0.99f; // Determine the slipperiness.

    public readonly float rangeRadius = 12.0f;


    // components
    [NonSerialized] public GameObject playerObj;
    [NonSerialized] public Rigidbody playerRd;

    // parents
    public Transform bulletsParent;
    public Transform pickersParent;

    // prefabs
    public GameObject bulletPrefab;
    public GameObject pickerPrefab;

    public PlayerInfo(){ } // A serializable class requires a default constructor

    public void Init(GameObject playerObj)
    {
        this.playerObj = playerObj;
        this.playerRd = playerObj.GetComponent<Rigidbody>();
    }

}


public abstract class PlayerUnit
{
    protected PlayerInfo info;


    protected PlayerUnit(PlayerInfo info)
    {
        this.info = info;
    }

    public abstract void MoveUnit(Vector3 direction);
    public abstract void UnitAction();

}

public class PlayerShooter
{
    PlayerInfo info;

    bool isShooting;
    float shootInterval = 0.5f;

    public PlayerShooter(PlayerInfo info)
    {
        this.info = info;
    }

    public GameObject FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(info.playerObj.transform.position, info.rangeRadius);
        var enemys = colliders.
            Where(collider => collider.CompareTag("Enemy")).
            Select(collider => collider.gameObject);

        return enemys.Any() ? enemys.First() : null;
    }

    public async void ShootEnemy(GameObject targetEnemy)
    {
        if (targetEnemy == null) return;

        if (isShooting) return;
        isShooting = true;

        // Debug.Log($"ShootEnemy() targetEnemy:{targetEnemy}");
        var bulletInitPos = info.bulletOffset * (targetEnemy.gameObject.transform.position - info.playerObj.transform.position).normalized + info.playerObj.transform.position;
        var bullet = Object.Instantiate(info.bulletPrefab, bulletInitPos, Quaternion.identity, info.bulletsParent).GetComponent<BulletController>();
        bullet.Init(targetEnemy.gameObject);

        for (float t = 0; t < shootInterval; t += Time.deltaTime) await UniTask.Yield();
        isShooting = false;

    }
}


public class PlayerTank : PlayerUnit
{
    float pickerHeight = 5.0f;

    public PlayerTank(PlayerInfo info): base(info)
    {

    }

    public override void MoveUnit(Vector3 direction)
    {
        info. playerRd.AddForce(info.acceleration * direction, ForceMode.Acceleration);
        if (info.playerRd.velocity.magnitude >= info.maxVelocity) info.playerRd.velocity = info.maxVelocity * info.playerRd.velocity.normalized;
        if (direction == Vector3.zero) info.playerRd.velocity = info.resistance * info.playerRd.velocity; //Decelerate when there is no key input
    }
    public override void UnitAction()
    {
        // Launch a picker.
        var pickerPos = info. playerObj.transform.position + new Vector3(0, pickerHeight, 0);
        var picker = Object.Instantiate(info.pickerPrefab, pickerPos, Quaternion.identity, info.pickersParent).GetComponent<PickerController>();
        picker.Init(info.playerObj.gameObject, info.rangeRadius);
    }
}

public class PlayerPlane : PlayerUnit
{
    bool isCollecting;
    float collectTime = 1f;
    float collectOffset = 0.5f; // determine how much to place the resource below.
    float detectionRange = 3f;



    public PlayerPlane(PlayerInfo info) : base(info)
    {

    }
    public override void MoveUnit(Vector3 direction)
    {
        info.playerRd.AddForce(info.acceleration * direction, ForceMode.Acceleration);
        if (info.playerRd.velocity.magnitude >= info.maxVelocity) info.playerRd.velocity = info.maxVelocity * info.playerRd.velocity.normalized;
        if (direction == Vector3.zero) info.playerRd.velocity = info.resistance * info.playerRd.velocity; //Decelerate when there is no key input
    }

    public override void UnitAction()
    {
        // Collect resource.
        var resource = FindAvailableResource();
        CollectResource(resource);
    }
    GameObject FindAvailableResource()
    {
        Collider[] colliders = Physics.OverlapSphere(Utility.SetYToZero( info.playerObj.transform.position), detectionRange);
        var resources = colliders.
            Where(collider => collider.CompareTag("Resource")).
            Where(collider => collider.gameObject.GetComponent<ResourceController>().isOwned == false).
            Select(collider => collider.gameObject);
        return resources.Any() ? resources.First() : null;
    }
    async void CollectResource(GameObject resource)
    {
        if(resource == null)return;
        if (isCollecting) return;

        var initPos = info.playerObj.transform.position;
        var deltaVector = resource.transform.position - initPos;

        isCollecting = true;

        for (float t = 0; t < collectTime; t += Time.deltaTime)
        {
            var coefficient = 2 * Mathf.PI / collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;

            info.playerObj.transform.position = progress * deltaVector + initPos;

            await UniTask.Yield();
        }

        Debug.Log("complete collect");
        resource.transform.position = info.playerObj.transform.position - new Vector3(0, collectOffset, 0);
        resource.transform.parent = info.playerObj.transform;

        isCollecting = false;
    }

}