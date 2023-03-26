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
    //Œã‚Å•Ï‚¦‚éBfloat‚Ì‚â‚èæ‚è‚Í‚µ‚½‚­‚È‚¢
    public float horizontal;
    public float vertical;
    public NetworkButtons buttons;
}
