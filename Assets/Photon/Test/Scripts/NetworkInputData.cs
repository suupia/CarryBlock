using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public const byte MOUSEBUTTON1 = 0x1;
    //Fusion �͓��͂����k���A���ۂɕω�����f�[�^�݂̂𑗐M����d�g�݂ɂȂ��Ă���
    public Vector3 direction;
    public byte buttons;
}
