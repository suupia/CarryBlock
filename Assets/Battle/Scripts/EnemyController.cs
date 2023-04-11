using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class EnemyController : MonoBehaviour
{
    bool isInitialized = false; 

    Transform resourcesParent;
    [SerializeField] GameObject resourcePrefab;

    float detectionRange = 30;

    GameObject targetPlayerObj;
    Rigidbody enemyRd;

    float acceleration = 15f;
    float maxVelocity = 10f;

    public enum EnemyState
    {
        idle,chasingPlayer,
    }

    EnemyState state = EnemyState.idle;

    public void  Init(Transform resourcesParent)
    {
        this.resourcesParent = resourcesParent;
        enemyRd = gameObject.GetComponent<Rigidbody>();
        isInitialized = true;
    }

    void Update()
    {
        if(!isInitialized)return;
        
        switch (state)
        {
            case EnemyState.idle:
                Search();
                break;
            case EnemyState.chasingPlayer:
                Chase();
                break;
        }
    }

    void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, detectionRange);
        var players = colliders.
            Where(collider => collider.CompareTag("Player")).
            Select(collider => collider.gameObject);
        if (players.Any())
        {
            targetPlayerObj = players.First();
            state = EnemyState.chasingPlayer;
        }
        else
        {
            state = EnemyState.idle;
        }
    }

    void Chase()
    {
        var directionVec = Utility.SetYToZero(targetPlayerObj.transform.position - gameObject.transform.position).normalized;
        enemyRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (enemyRd.velocity.magnitude >= maxVelocity) enemyRd.velocity = maxVelocity* enemyRd.velocity.normalized;
    }

    public void OnDefeated()
    {
        Instantiate(resourcePrefab,transform.position, Quaternion.identity,resourcesParent);
        Destroy(this.gameObject);
    }

}
