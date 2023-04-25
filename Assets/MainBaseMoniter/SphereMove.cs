using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        var movement = new Vector3(dx, 0, dz);
        rb.AddForce(movement * 10.0f);
    }
}
