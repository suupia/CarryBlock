using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject bulletGameObj;

    Rigidbody playerRd;
    float speed = 100;

    void Start()
    {
        playerRd = GetComponent<Rigidbody>();
        Debug.Log($"{playerRd}");

    }

    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        var moveVector = new Vector3(horizontalInput, 0, verticalInput);

        transform.position = transform.position + moveVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Enemy"))
        {
            var bullet = Instantiate(bulletGameObj,transform.position,Quaternion.identity).GetComponent<BulletController>();
            bullet.Init(other.gameObject);
        }
    }
}
