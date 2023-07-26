using Fusion;
#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartControllerNet : NetworkBehaviour
    {
        CartShortestRouteMove _move; 
        public void Init(CartShortestRouteMove move)
        {
            _move = move;
        }

        public void StartMove()
        {
            _move.MoveAlongWithShortestRoute();
        }
    }
}