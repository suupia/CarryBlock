namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveChainable
    {
        public IMoveParameter Chain(IMoveParameter parameter);
        public IMoveFunction Chain(IMoveFunction function);

    }
}