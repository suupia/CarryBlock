using System;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Projects.CarrySystem.Enemy;
using VContainer;
using DG.Tweening;
using UnityEngine;

#nullable enable

namespace Carry.UISystem.UI.LobbyScene
{
    public class LobbyStartGameTheater
    {
        readonly CartLobbyControllerNet _cart;
        readonly EnemyControllerNet _enemy;
        readonly LobbyPlayerContainer _lobbyPlayerContainer = null!;


        [Inject]
        public LobbyStartGameTheater(CartLobbyControllerNet cart, EnemyControllerNet enemy)
        {
            _cart = cart;
            _enemy = enemy;
        }

        public void PlayLobbyTheater(Action onCompleteAction)
        {
            var enemyAnimatorPresenter = _enemy.GetComponentInChildren<EnemyAnimatorPresenterNet>();

            var animationSequence = DOTween.Sequence();

            animationSequence = AppendPlayerGetIntoCartAnimation(animationSequence);

            animationSequence = AppendEnemyApproachAnimation(animationSequence, enemyAnimatorPresenter);

            animationSequence = AppendPlayerLookAtEnemyAnimation(animationSequence);

            animationSequence = AppendEnemyAndCartMoveAnimation(animationSequence, enemyAnimatorPresenter);

            animationSequence.OnComplete(() => onCompleteAction());

            animationSequence.Play();
        }

        Vector3 CalcPlayerPositionInCart(int playerIndex, Vector3 cartPosition)
        {
            return playerIndex switch
            {
                0 => cartPosition + new Vector3(0.7f, 0, 0.7f),
                1 => cartPosition + new Vector3(0.7f, 0, -0.7f),
                2 => cartPosition + new Vector3(-0.7f, 0, 0.7f),
                3 => cartPosition + new Vector3(-0.7f, 0, -0.7f),
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }


        Sequence AppendPlayerGetIntoCartAnimation(Sequence sequence)
        {
            for (int playerIndex = 0;
                 playerIndex < _lobbyPlayerContainer.PlayerControllers.Count;
                 playerIndex++)
            {
                var playerController = _lobbyPlayerContainer.PlayerControllers[playerIndex];
                var playerTransform = playerController.transform;

                sequence
                    .Append(playerTransform.DOLookAt(
                        CalcPlayerPositionInCart(playerIndex, _cart.transform.position), 0))
                    .AppendCallback(() => playerController.GetCharacter.Dash())
                    .Append(playerTransform.DOMove(
                        CalcPlayerPositionInCart(playerIndex, _cart.transform.position), 2f))
                    .AppendCallback(() =>
                    {
                        playerController.GetCharacter.SwitchToRegularMove();
                        var changePosition = new Vector3(playerTransform.position.x, 1.0f, playerTransform.position.z);
                        playerTransform.position = changePosition;
                        playerTransform.SetParent(_cart.transform);
                    });
            }

            return sequence;
        }

        Sequence AppendEnemyApproachAnimation(Sequence sequence, EnemyAnimatorPresenterNet enemyAnimatorPresenter)
        {
            sequence
                .Append(_enemy.transform.DOLookAt(_cart.transform.position, 0))
                .AppendCallback(() => enemyAnimatorPresenter.Chase())
                .Append(_enemy.transform.DOMove(new Vector3(-6f, 0, 0f), 1.5f))
                .AppendCallback(() => enemyAnimatorPresenter.Threat())
                .Append(_enemy.transform.DOMove(new Vector3(-5.5f, 0, 0f), 1.5f));
            return sequence;
        }

        Sequence AppendPlayerLookAtEnemyAnimation(Sequence sequence)
        {
            foreach (var playerController in _lobbyPlayerContainer.PlayerControllers)
            {
                sequence.Append(playerController.transform.DOLookAt(new Vector3(40f, 0, 0), 0));
            }

            return sequence;
        }

        Sequence AppendEnemyAndCartMoveAnimation(Sequence sequence, EnemyAnimatorPresenterNet enemyAnimatorPresenter)
        {
            sequence
                .AppendCallback(() => enemyAnimatorPresenter.Chase())
                .Append(_enemy.transform.DOMove(new Vector3(15f, 0, 0f), 2.5f))
                .Join(_cart.transform.DOMove(new Vector3(40f, 0, 0), 2.5f));
            return sequence;
        }
    }
}