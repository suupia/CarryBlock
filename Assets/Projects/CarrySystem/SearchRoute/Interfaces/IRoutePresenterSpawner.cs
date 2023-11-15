using UnityEngine;

namespace Carry.CarrySystem.RoutingAlgorithm.Interfaces
{
    // クライアントコードにおいて、foreachでDestroyやDespawnを使い分ける取ったことがなければこのインターフェースが使える
    public interface IRoutePresenterSpawner
    {
        public IRoutePresenter SpawnIRoutePresenter(Vector3 position, Quaternion rotation);
    }
}