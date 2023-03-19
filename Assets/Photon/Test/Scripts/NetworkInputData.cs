using Fusion;
using UnityEngine;

public enum PlayerOperations
{
    Attack = 0,
    Forward,
    Backward,
    Left,
    Right,
}

public struct NetworkInputData : INetworkInput
{
    //Fusion �͓��͂����k���A���ۂɕω�����f�[�^�݂̂𑗐M����d�g�݂ɂȂ��Ă���
    public NetworkButtons buttons;
}
