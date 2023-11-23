namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveRecord
    {
        public IMoveParameter Chain(IMoveParameter parameter);
        public IMoveFunction Chain(IMoveFunction function);

    }
}