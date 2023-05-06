using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Animations.Tests
{
    public class TestPlayerController : NetworkBehaviour
    {
        [SerializeField] private GameObject planePrefab;
        private PlayerDecorationDetector _playerDecorationDetector;

        private GameObject _playerUnitObject;
        [Networked] private NetworkButtons PreButtons { get; set; }

        [Networked]
        private ref PlayerDecorationDetector.Data DecorationDataRef => ref MakeRef<PlayerDecorationDetector.Data>();

        //以下はテスト用のプロパティ
        [Networked] private byte Hp { get; set; } = 2;

        private void Update()
        {
        }

        public override void Spawned()
        {
            Setup();
            _playerDecorationDetector.OnSpawned();
        }

        private void Setup()
        {
            _playerUnitObject = Instantiate(planePrefab, transform);
            _playerDecorationDetector = new PlayerDecorationDetector(
                new PlaneAnimatorSetter(_playerUnitObject));
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                    _playerDecorationDetector.OnMainAction(ref DecorationDataRef);

                //Assuming changed transform.forward
                //現状のテストだとNetworkTransform系がついていないので完全に同期はしない可能性がある。あくまでテスト用のコード
                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                _playerDecorationDetector.OnChangeDirection(ref DecorationDataRef, direction);

                //Assuming Attacked
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug1))
                    _playerDecorationDetector.OnAttacked(ref DecorationDataRef);

                //Assuming Dead
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug2)) Hp--;

                PreButtons = input.Buttons;
            }
        }

        public override void Render()
        {
            //Decoration側の理想としては、OnRenderedの中でOnMovedが呼ばれる
            //そのため、動き系の処理と同じループ頻度でOnRenderedは呼んでほしい
            //ただ、トルク系は動きが残るので大丈夫かも。要検討
            _playerDecorationDetector.OnRendered(ref DecorationDataRef, Hp);
        }
    }
}