using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<CartControllerNet> _cartControllerLoader;

        [Inject]
        public CartBuilder(
            NetworkRunner runner ,
            IPrefabLoader<CartControllerNet> cartControllerLoader
            )
        {
            _runner = runner;
            _cartControllerLoader = cartControllerLoader;
        }

        public CartControllerNet Build(SquareGridMap map)
        {
            Debug.Log($"CartBuilder.Build");
            // プレハブをロード
            var cartController = _cartControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var move = new CartShortestRouteMove();
            
            // プレハブをスポーン
            var startPos = new Vector2Int(1, map.Height % 2 == 1 ? (map.Height - 1) / 2 : map.Height / 2);
            // Debug.Log($"startPos: {startPos}");
            var position = GridConverter.GridPositionToWorldPosition(startPos);
            var cartControllerObj = _runner.Spawn(cartController,position, Quaternion.identity, PlayerRef.None,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, cartControllerObj");
                    networkObj.GetComponent<CartControllerNet>().Init(move);
                    // networkObj.GetComponent<HoldPresenter_Net>().Init(character);
                });
            
            // 各MonoBehaviourにドメインを設定
            // playerControllerObj.Init(character);
            // var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            // holdPresenter.Init(character);
            
            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return cartControllerObj;
        }
    }
}