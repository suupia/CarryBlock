using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour
{
    public enum PickerState
    {
        Approach, Collect, Return
    }

    public PickerState state = PickerState.Approach;

    public void Init()
    {
        
    }

    private void SearchAgain()
    {
        //�ŏ��Ƀ^�[�Q�b�g�ɂ��Ă���������������ꂽ���ɌĂяo�����
    }
}
