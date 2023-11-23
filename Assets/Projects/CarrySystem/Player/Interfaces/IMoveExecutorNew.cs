namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorNew
    {
        public IMoveParameter Chain(IMoveParameter parameter);
        public IMoveFunction Chain(IMoveFunction nextMoveFunction);

    }
}