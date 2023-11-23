namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorNew
    {
        public IMoveParameter Chain(IMoveParameter nextMoveParameter);
        public IMoveFunction Chain(IMoveFunction nextMoveFunction);

    }
}