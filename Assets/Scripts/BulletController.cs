using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    GameObject targetGameObj;
    float speed = 30;
    Rigidbody bulletRd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init(GameObject targetGameObj)
    {
        this.targetGameObj = targetGameObj;
        var directionVec = (targetGameObj.transform.position - transform.position).normalized;
        bulletRd = GetComponent<Rigidbody>();
        bulletRd.AddForce(speed*directionVec , ForceMode.Impulse);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
