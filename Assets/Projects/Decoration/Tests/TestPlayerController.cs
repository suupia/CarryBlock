
using Decoration;
using Fusion;
using UnityEngine;
using Main;

namespace Animations.Tests
{
    public class TestPlayerController : NetworkBehaviour
    {
        [SerializeField] GameObject planePrefab;
        [Networked] NetworkButtons PreButtons { get; set; }

        [Networked] 
        ref NetworkDecorationPlayer DecorationPlayerRef => ref MakeRef<NetworkDecorationPlayer>();

        //以下はテスト用のプロパティ
        [Networked] byte Hp { get; set; }
        [Networked] float Horizontal { get; set; }

        GameObject _playerUnitObject;
        DecorationPlayerContainer _decorationContainer;

        public override void Spawned()
        {
            Setup();
            _decorationContainer.OnSpawn();
        }

        void Setup()
        {
            _playerUnitObject = Instantiate(planePrefab, transform);
            _decorationContainer = new DecorationPlayerContainer(
                new PlaneAnimatorSetter(_playerUnitObject));
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    _decorationContainer.OnMainAction(ref DecorationPlayerRef);
                }

                Horizontal = input.Horizontal;
                
                //Assuming Attacked
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug1))
                {
                    _decorationContainer.OnAttack(ref DecorationPlayerRef);
                }
                
                //Assuming Dead
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug2))
                {
                    Hp = 0;
                }

                PreButtons = input.Buttons;
            }
        }

        public override void Render()
        {
            //Assuming changed transform.forward
            var preRotation = _playerUnitObject.transform.rotation.eulerAngles;
            var rotation = Quaternion.Euler(0, preRotation.y + Horizontal, 0);
            _playerUnitObject.transform.rotation = rotation;

            _decorationContainer.OnRender(ref DecorationPlayerRef, Hp);
        }
    }
}