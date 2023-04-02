using Fusion;

public enum PlayerOperation
{
    MainAction = 0,
    Ready,
    ChangeUnit,
}

public struct NetworkInputData : INetworkInput
{
    //後で変える。floatのやり取りはしたくない
    public float horizontal;
    public float vertical;
    public NetworkButtons buttons;
}
