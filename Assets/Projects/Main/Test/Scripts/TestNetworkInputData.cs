using Fusion;


public enum TestPlayerOperations
{
    Attack = 0,
    Forward,
    Backward,
    Left,
    Right,
}

public struct TestNetworkInputData : INetworkInput
{
    //Fusion は入力を圧縮し、実際に変化するデータのみを送信する仕組みになっている
    public NetworkButtons buttons;
}
