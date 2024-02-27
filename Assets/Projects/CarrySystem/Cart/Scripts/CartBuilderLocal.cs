#nullable enable
using Carry.CarrySystem.Cart.Info;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.Utility.Interfaces;
using Fusion;
using Projects.CarrySystem.Cart.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartBuilderLocal : ICartBuilder
    {
        readonly IPrefabLoader<CartControllerLocal> _cartControllerLoader;
        readonly CartShortestRouteMove _cartShortestRouteMove;

        [Inject]
        public CartBuilderLocal(
            IPrefabLoader<CartControllerLocal> cartControllerLoader,
            CartShortestRouteMove cartShortestRouteShortestRouteMove
            )
        {
            _cartControllerLoader = cartControllerLoader;
            _cartShortestRouteMove = cartShortestRouteShortestRouteMove;
        }

        public void Build(EntityGridMap map, IMapSwitcher mapSwitcher)
        {
            Debug.Log($"CartBuilder.Build");
            
            // 前のカートを削除
            // ToDo: 移動に切り替える
            var prevCart = GameObject.FindObjectOfType<CartControllerNet>();
            if(prevCart != null) UnityEngine.Object.Destroy(prevCart.Object);
            
            
            // プレハブをロード
            var cartController = _cartControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var startPos = new Vector2Int(1, map.Height / 2);
            var endPos = new Vector2Int(map.Width -1 ,  map.Height / 2);
            
            // CartShortestRouteMoveにRegiste
            _cartShortestRouteMove.RegisterMap(map);
            _cartShortestRouteMove.RegisterIMapUpdater(mapSwitcher); // ToDo: これはまずそう　何とかする
            
            // プレハブをスポーン
            var position = GridConverter.GridPositionToWorldPosition(startPos);
            // var cartControllerObj = _runner.Spawn(cartController,position, Quaternion.identity, PlayerRef.None,
            //     (runner, networkObj) =>
            //     {
            //         Debug.Log($"OnBeforeSpawn: {networkObj}, cartControllerObj");
            //         networkObj.GetComponent<CartControllerNet>().Init(_cartShortestRouteMove);
            //         // networkObj.GetComponent<HoldPresenter_Net>().Init(character);
            //     });
            var cartControllerObj = UnityEngine.Object.Instantiate(cartController, position, Quaternion.identity);
            cartControllerObj.Init(_cartShortestRouteMove);
            
            // Infoを設定
            var cartInfo = new CartInfo(cartControllerObj.gameObject);
            _cartShortestRouteMove.Setup(cartInfo);
            
            // 各MonoBehaviourにドメインを設定
            // playerControllerObj.Init(character);
            // var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            // holdPresenter.Init(character);
            
            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

        }
    }
}