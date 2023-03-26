using Fusion;

public enum PlayerOperation
{
    MainAction = 0,
    Fire,
    Ready,
    ChangeUnit,
}

public struct NetworkInputData : INetworkInput
{
    //��ŕς���Bfloat�̂����͂������Ȃ�
    public float horizontal;
    public float vertical;
    public NetworkButtons buttons;
}
