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

    public enum UnitType
    {
        Tank, Plane,
    }


    void Start()
    {

        playerObj = Instantiate(unitPrefabs[(int)unitType]);

        Instantiate(cameraPrefab, playerObj.transform);
        var rangeCircleGameObj = Instantiate(rangeCirclePrefab, playerObj.transform);


        bulletsParent = GameObject.Find("BulletsParent").transform;
        pickersParent = GameObject.Find("PickersParent").transform;


        playerRd  = playerObj.GetComponent<Rigidbody>();

        rangeCircleGameObj.transform.localScale = new Vector3(rangeRadius*2,rangeRadius*2,rangeRadius * 2); // localScale sets the diameter
        var onTriggerEnterComponent = rangeCircleGameObj.GetComponent<OnTriggerEnterComponent>();
        Debug.Log($"onTriggerEnterComponent:{onTriggerEnterComponent}");
        onTriggerEnterComponent.SetOnTriggerEnterAction(OnTriggerRangeCircle);
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
            LaunchPicker();
        }
    }


    private void OnTriggerRangeCircle(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var bulletInitPos = bulletOffset * (other.gameObject.transform.position - playerObj.transform.position).normalized + playerObj.transform.position;
            var bullet = Instantiate(bulletPrefab, bulletInitPos, Quaternion.identity, bulletsParent).GetComponent<BulletController>();
            bullet.Init(other.gameObject);
        }
    }

    private void LaunchPicker()
    {
        var pickerPos = playerObj.transform.position + new Vector3(0, pickerHeight, 0);
        var picker = Instantiate(pickerPrefab, pickerPos, Quaternion.identity,pickersParent).GetComponent<PickerController>();
        picker.Init(playerObj.gameObject, rangeRadius);
    }
}
