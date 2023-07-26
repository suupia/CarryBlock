﻿using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Projects.CarrySystem.Block.Interfaces;
using Projects.CarrySystem.Cart.Info;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;
using VContainer;

#nullable enable
namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartShortestRouteMove
    {
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        EntityGridMap? _map; // このクラスはMapを登録して使用する (コンストラクタでIMapUpdaterを受け取らない)

        CartInfo? _info ;
        Direction _direction;

        enum Direction
        {
            Front,
            Back,
            Right,
            Left,
            DiagRightFront,
            DiagLeftFront,
            DiagRightBack,
            DiagLeftBack
        }

        [Inject]
        public CartShortestRouteMove(
            ReachRightEdgeChecker reachRightEdgeChecker
        )
        {
            _reachRightEdgeChecker = reachRightEdgeChecker;
        }

        public void Setup(CartInfo info)
        {
            _info = info;
        }

        public void RegisterMap(EntityGridMap map)
        {
            _map = map;
        }

        public void MoveAlongWithShortestRoute()
        {
            if (_map == null)
            {
                Debug.LogError($"_map is null");
                return;
            }

            var routes = SearchShortestRoute();
            
            Debug.Log($"routes : {string.Join(",", routes)}");
            Move(routes);


        }

        List<Vector2Int> SearchShortestRoute()
        {
            var searcherSize = SearcherSize.SizeThree;

            var waveletSearchExecutor = new WaveletSearchExecutor(_map); // RoutePresenterをかませる必要がないから直接new
            var searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var startPos = new Vector2Int(1, _map.Height % 2 == 1 ? (_map.Height - 1) / 2 : _map.Height / 2);
            var endPos = new Vector2Int(_map.Width - 2,
                _map.Height % 2 == 1 ? (_map.Height - 1) / 2 : _map.Height / 2); // ToDo: どこかで統一して宣言する
            Func<int, int, bool> isWall = (x, y) => _map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            var accessibleArea = waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall, searcherSize);
            var endPosY = _reachRightEdgeChecker.CalcCartReachRightEdge(accessibleArea, _map, searcherSize);
            var routeEndPos = new Vector2Int(endPos.x, endPosY);

            var routes = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, routeEndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, isWall, searcherSize);

            return routes;
        }

        async UniTaskVoid Move(List<Vector2Int> routes)
        {
            var beforeGridPos = routes.First();
            foreach (var route in routes)
            {
                var diff = route - beforeGridPos;
                SetDirection(diff);  // ToDo: 向きを変更
                await MoveTo(beforeGridPos, route);
            }

            async UniTask MoveTo(Vector2Int startGridPos, Vector2Int endGridPos)
            {
                var startPos = GridConverter.GridPositionToWorldPosition(startGridPos);
                var endPos = GridConverter.GridPositionToWorldPosition(endGridPos);
                await _info.cartObj.transform.DOMove(endPos, 0.15f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }

            void SetDirection(Vector2 directionVector)
            {
                if (directionVector == Vector2.zero) //引数の方向ベクトルがゼロベクトルの時は何もしない
                {
                    return;
                }
                float angle = Vector2.SignedAngle(Vector2.right, directionVector);
                //Debug.Log($"SetDirectionのangleは{angle}です");

                //directionとanimationを決定する
                if (-22.5f <= angle && angle < 22.5f)
                {
                    _direction = Direction.Right;
                }
                else if (22.5f <= angle && angle < 67.5f)
                {
                    _direction = Direction.DiagRightBack;
                }
                else if (67.5f <= angle && angle < 112.5f)
                {
                    _direction = Direction.Back;
                }
                else if (112.5f <= angle && angle < 157.5f)
                {
                    _direction = Direction.DiagLeftBack;

                }
                else if (-157.5f <= angle && angle < -112.5f)
                {
                    _direction = Direction.DiagLeftFront;
                }
                else if (-112.5f <= angle && angle < -67.5f)
                {
                    _direction = Direction.Front;
                }
                else if (-67.5f <= angle && angle < -22.5f)
                {
                    _direction = Direction.DiagRightFront;
                }
                else //角度は-180から180までで端点は含まないらしい。そのため、Direction.Leftはelseで処理することにした。
                {
                    _direction = Direction.Left;
                }
            }
        }
    }
}