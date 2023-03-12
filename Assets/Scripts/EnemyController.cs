using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;

    public void OnDefeated()
    {
        Instantiate(resourcePrefab,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
}
