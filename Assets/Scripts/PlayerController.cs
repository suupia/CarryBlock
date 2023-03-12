using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform bulletsParent;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform pickersParent;
    [SerializeField] GameObject pickerPrefab;

    Rigidbody playerRd;
    float acceleration = 10f;
    float maxVelocity = 30;
    float bulletOffset = 1;
    float resistance = 0.99f;


    //range
    float rangeRadius = 10.5f;

    void Start()
    {
        playerRd = GetComponent<Rigidbody>();
        Debug.Log($"{playerRd}");

        var rangeCircleGameObj = GameObject.Find("RangeCircle");
        var onTriggerEnterComponet = rangeCircleGameObj.GetComponent<OnTriggerEnterComponet>();
        rangeCircleGameObj.transform.localScale = new Vector3(rangeRadius*2,rangeRadius*2,rangeRadius * 2); // localScale sets the diameter
        onTriggerEnterComponet.SetOnTriggerEnterAction(OnTriggerRangeCircle);
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
            var bulletInitPos = bulletOffset * (other.gameObject.transform.position - transform.position).normalized + transform.position;
            var bullet = Instantiate(bulletPrefab, bulletInitPos, Quaternion.identity, bulletsParent).GetComponent<BulletController>();
            bullet.Init(other.gameObject);
        }
    }

    private void LaunchPicker()
    {
        var picker = Instantiate(pickerPrefab,transform.position,Quaternion.identity,pickersParent).transform.Find("Picker").GetComponent<PickerController>();
        picker.Init(rangeRadius);
    }
}
