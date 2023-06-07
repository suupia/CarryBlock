using Fusion;

public enum TestPlayerOperations
{
    Attack = 0,
    Forward,
    Backward,
    Left,
    Right
}

public struct TestNetworkInputData : INetworkInput
{
    //Fusion �͓��͂����k���A���ۂɕω�����f�[�^�݂̂𑗐M����d�g�݂ɂȂ��Ă���
    public NetworkButtons buttons;
}