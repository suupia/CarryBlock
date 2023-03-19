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
    //Fusion は入力を圧縮し、実際に変化するデータのみを送信する仕組みになっている
    public NetworkButtons buttons;
}
