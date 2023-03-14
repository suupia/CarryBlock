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

    GameObject playerObj;
    Rigidbody playerRd;
    float acceleration = 10f;
    float maxVelocity = 15;
    float bulletOffset = 1;
    float resistance = 0.99f;


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
        playerObj = Instantiate(unitPrefabs[(int)unitType],gameObject.transform);

        Instantiate(cameraPrefab, playerObj.transform);
        var rangeCircleGameObj = Instantiate(rangeCirclePrefab, playerObj.transform);


        bulletsParent = GameObject.Find("BulletsParent").transform;
        pickersParent = GameObject.Find("PickersParent").transform;


        playerRd  = playerObj.GetComponent<Rigidbody>();

        rangeCircleGameObj.transform.localScale = new Vector3(rangeRadius*2,rangeRadius*2,rangeRadius * 2); // localScale sets the diameter
        var onTriggerEnterComponent = rangeCircleGameObj.GetComponent<OnTriggerEnterComponent>();
        Debug.Log($"onTriggerEnterComponent:{onTriggerEnterComponent}");
        onTriggerEnterComponent.SetOnTriggerEnterAction(OnTriggerRangeCircle);



        var info = new PlayerInfo(playerObj);
        info.SetBulletsParent(bulletsParent);
        info.SetPickerParent(pickersParent);
        info.SetBulletPrefab(bulletPrefab);
        info.SetPickerPrefab(pickerPrefab);
        playerUnit = new PlayerTank(info);
        
    }

    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        var directionVec = Vector3.Normalize(new Vector3(horizontalInput, 0, verticalInput));


        // related to movement

        //transform.position = speed * directionVec + transform.position;
        playerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (playerRd.velocity.magnitude >= maxVelocity) playerRd.velocity = maxVelocity * playerRd.velocity.normalized;

        if (directionVec == Vector3.zero) playerRd.velocity = resistance * playerRd.velocity; //Decelerate when there is no key input

        // related to action

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button is clicked.");
            //LaunchPicker();
            playerUnit.UnitAction();
        }
    }


    void OnTriggerRangeCircle(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var bulletInitPos = bulletOffset * (other.gameObject.transform.position - playerObj.transform.position).normalized + playerObj.transform.position;
            var bullet = Instantiate(bulletPrefab, bulletInitPos, Quaternion.identity, bulletsParent).GetComponent<BulletController>();
            bullet.Init(other.gameObject);
        }
    }

    void LaunchPicker()
    {
        var pickerPos = playerObj.transform.position + new Vector3(0, pickerHeight, 0);
        var picker = Instantiate(pickerPrefab, pickerPos, Quaternion.identity,pickersParent).GetComponent<PickerController>();
        picker.Init(playerObj.gameObject, rangeRadius);
    }

    void CollectResource()
    {

    }
}
public class PlayerInfo
{
    // constant fields
    public readonly float acceleration = 10f;
    public readonly float maxVelocity = 15;
    public readonly float bulletOffset = 1;
    public readonly float resistance = 0.99f; // Determine the slipperiness.

    public readonly float rangeRadius = 12.0f;


    // components
    public readonly GameObject playerObj;
    public readonly Rigidbody playerRd;

    // injected fields
    // ..

    // parents
    public Transform bulletsParent;
    public Transform pickersParent;

    // prefabs
    public GameObject bulletPrefab;
    public GameObject pickerPrefab;



    public PlayerInfo(GameObject playerObj)
    {
        this.playerObj = playerObj;
        this.playerRd = playerObj.GetComponent<Rigidbody>();
    }

    public void  SetBulletsParent(Transform bulletsParent)
    {
        this.bulletsParent = bulletsParent;
    }
    public void SetPickerParent(Transform pickersParent)
    {
        this.pickersParent = pickersParent;
    }

    public void SetBulletPrefab(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public void SetPickerPrefab(GameObject pickerPrefab)
    {
        this.pickerPrefab = pickerPrefab;
    }

}


public abstract class PlayerUnit
{
    protected PlayerInfo info;

    protected PlayerUnit(PlayerInfo info)
    {
        this.info = info;
    }
    public abstract void UnitAction();
}

public class PlayerTank : PlayerUnit
{
    float pickerHeight = 5.0f;

    public PlayerTank(PlayerInfo info): base(info)
    {

    }

    public override void UnitAction()
    {
        // Launch a picker.
        var pickerPos = info. playerObj.transform.position + new Vector3(0, pickerHeight, 0);
        var picker = MonoBehaviour.Instantiate(info.pickerPrefab, pickerPos, Quaternion.identity, info.pickersParent).GetComponent<PickerController>();
        picker.Init(info.playerObj.gameObject, info.rangeRadius);
    }
}

public class PlayerPlane : PlayerUnit
{
    public PlayerPlane(PlayerInfo info) : base(info)
    {

    }

    public override void UnitAction()
    {

    }
}