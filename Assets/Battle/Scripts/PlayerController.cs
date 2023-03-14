using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject rangeCirclePrefab;

    Transform bulletsParent;
    [SerializeField] GameObject bulletPrefab;
    Transform pickersParent;
    [SerializeField] GameObject pickerPrefab;

    [SerializeField] GameObject[] unitPrefabs;

    [SerializeField] UnitType unitType;

    [SerializeField] PlayerInfo info;


    //range
    float rangeRadius = 12.0f;

    //picker
    float pickerHeight = 5.0f;


    PlayerUnit playerUnit;

    public enum UnitType
    {
        Tank, Plane,
    }


    void Start()
    {
        var playerObj = Instantiate(unitPrefabs[(int)unitType],gameObject.transform);

        Instantiate(cameraPrefab, playerObj.transform);
        var rangeCircleGameObj = Instantiate(rangeCirclePrefab, playerObj.transform);


        bulletsParent = GameObject.Find("BulletsParent").transform;
        pickersParent = GameObject.Find("PickersParent").transform;


        info.Init(playerObj);
        playerUnit = new PlayerTank(info);


        rangeCircleGameObj.transform.localScale = new Vector3(rangeRadius*2,rangeRadius*2,rangeRadius * 2); // localScale sets the diameter
        var onTriggerEnterComponent = rangeCircleGameObj.GetComponent<OnTriggerEnterComponent>();
        Debug.Log($"onTriggerEnterComponent:{onTriggerEnterComponent}");
        onTriggerEnterComponent.SetOnTriggerEnterAction(playerUnit.OnTriggerRangeCircle);


        
    }

    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        var direction = Vector3.Normalize(new Vector3(horizontalInput, 0, verticalInput));


        // related to movement

        ////transform.position = speed * directionVec + transform.position;
        //playerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        //if (playerRd.velocity.magnitude >= maxVelocity) playerRd.velocity = maxVelocity * playerRd.velocity.normalized;

        //if (directionVec == Vector3.zero) playerRd.velocity = resistance * playerRd.velocity; //Decelerate when there is no key input

        playerUnit.MoveUnit(direction);

        // related to action

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button is clicked.");
            playerUnit.UnitAction();
        }
    }


    //void OnTriggerRangeCircle(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        var bulletInitPos = bulletOffset * (other.gameObject.transform.position - playerObj.transform.position).normalized + playerObj.transform.position;
    //        var bullet = Instantiate(bulletPrefab, bulletInitPos, Quaternion.identity, bulletsParent).GetComponent<BulletController>();
    //        bullet.Init(other.gameObject);
    //    }
    //}

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

    public abstract void OnTriggerRangeCircle(Collider other);
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
        var picker = MonoBehaviour.Instantiate(info.pickerPrefab, pickerPos, Quaternion.identity, info.pickersParent).GetComponent<PickerController>();
        picker.Init(info.playerObj.gameObject, info.rangeRadius);
        Debug.Log($"rangeRadius:{info.rangeRadius}");
    }
    public override void OnTriggerRangeCircle(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var bulletInitPos = info. bulletOffset * (other.gameObject.transform.position - info.playerObj.transform.position).normalized + info.playerObj.transform.position;
            var bullet = MonoBehaviour.Instantiate(info.bulletPrefab, bulletInitPos, Quaternion.identity, info.bulletsParent).GetComponent<BulletController>();
            bullet.Init(other.gameObject);
        }
    }
}

public class PlayerPlane : PlayerUnit
{
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

    }

    public override void OnTriggerRangeCircle(Collider other)
    {

    }
}