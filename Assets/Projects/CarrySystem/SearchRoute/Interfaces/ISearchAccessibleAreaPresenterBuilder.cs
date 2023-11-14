using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;

namespace Carry.CarrySystem.RoutingAlgorithm.Interfaces
{
    public interface ISearchAccessibleAreaPresenterBuilder
    {
        public SearchAccessibleAreaPresenter BuildPresenter(IGridMap map);
    }
}