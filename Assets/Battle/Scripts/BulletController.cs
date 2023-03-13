using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    GameObject targetGameObj;
    Rigidbody bulletRd;

    float speed = 30;
    float lifeTime = 8;

    float timer = 0;

    public void Init(GameObject targetGameObj)
    {
        this.targetGameObj = targetGameObj;
        var directionVec = (targetGameObj.transform.position - transform.position).normalized;
        bulletRd = GetComponent<Rigidbody>();
        bulletRd.AddForce(speed*directionVec , ForceMode.Impulse);
    }

    private void Update()
    {
        timer += Time.fixedDeltaTime;
        if (timer > lifeTime) DestroyBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyController>();
            enemy.OnDefeated();
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
