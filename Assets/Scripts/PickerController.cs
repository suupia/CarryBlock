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
            // �����ΏۂƂȂ�I�u�W�F�N�g���擾����
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
        //�ŏ��Ƀ^�[�Q�b�g�ɂ��Ă���������������ꂽ���ɌĂяo�����

    }

    private void NotFoundProcess()
    {
        //�^���N�ɖ߂�
    }

    private void ApproachProcess()
    {
        //�����Ɍ�����
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
