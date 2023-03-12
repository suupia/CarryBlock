using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform resourcesParent;
    [SerializeField] GameObject resourcePrefab;

    public void  Init(Transform resourcesParent)
    {
        this.resourcesParent = resourcesParent;
    }
    public void OnDefeated()
    {
        Instantiate(resourcePrefab,transform.position, Quaternion.identity,resourcesParent);
        Destroy(this.gameObject);
    }
}
