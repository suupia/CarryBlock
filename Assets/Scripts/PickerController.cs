using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour
{

    float acceleration = 10f;
    float maxVelocity = 30;


    Rigidbody pickerRd;
    GameObject targetResource;
    float detectionRange;


    public enum PickerState
    {
        NotFound,Approach, Collect, Return
    }

    public PickerState state = PickerState.Approach;

    public void Init(float rangeRadius)
    {
        pickerRd = GetComponent<Rigidbody>();   
        detectionRange = rangeRadius;
        Search();
    }

    private void Update()
    {
        switch (state)
        {
            case PickerState.NotFound:

                break;
            case PickerState.Approach:
                ApproachProcess();
                break;
            case PickerState.Collect:
                break;
            case PickerState.Return:
                break;
        }
    }

    private void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            // 検索対象となるオブジェクトを取得する
            if (collider.CompareTag("Resource"))
            {
                targetResource = collider.gameObject;
                state = PickerState.Approach;
                break;
            }
            state = PickerState.NotFound;
        }
    }


    private void SearchAgain()
    {
        //最初にターゲットにしていた資源が回収された時に呼び出される

    }

    private void NotFoundProcess()
    {
        //タンクに戻る
    }

    private void ApproachProcess()
    {
        //資源に向かう
        var directionVec = Utility.SetYToZero(targetResource.transform.position - transform.position).normalized;
        pickerRd.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        if (pickerRd.velocity.magnitude >= maxVelocity) pickerRd.velocity = maxVelocity * pickerRd.velocity.normalized;

    }

    // Debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
