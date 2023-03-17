using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public const byte MOUSEBUTTON1 = 0x1;
    //Fusion は入力を圧縮し、実際に変化するデータのみを送信する仕組みになっている
    public Vector3 direction;
    public byte buttons;
}
