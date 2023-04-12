using System;
using System.Collections;
using System.Collections.Generic;
using Animations.Scripts;
using Fusion;
using UnityEngine;
using Network.AnimatorSetter;
using Network.AnimatorSetter.Info;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

interface IAnimatorMove
{
    /// <summary>
    /// Needs some param
    /// </summary>
    /// <param name="direction">Used for manage body animation</param>
    /// <param name="target">Used for shooter or some animation without body. This can be null</param>
    void OnMove(Vector3 direction, Transform target = null);
}

interface IAnimatorDead
{
    void OnDead();
}

interface IAnimatorSpawn
{
    void OnSpawn();
}

interface IAnimatorAttack
{
    void OnAttack(Transform target = null);
}

interface IAnimatorPlayerUnit: IAnimatorAttack, IAnimatorDead, IAnimatorMove, IAnimatorSpawn
{
    void OnMainAction();
}

namespace Network.Test
{
    
    public class TestController : NetworkBehaviour
    {
        private string DebugData => $"Tick.Row: {Runner.Tick.Raw}, frameCount: {Time.frameCount}";
        
        private IAnimatorPlayerUnit _animatorSetter;
        
        
        [SerializeField] private GameObject planePrefab;
        
        [Networked] private NetworkButtons PreButtons { get; set; }
        [Networked] private Vector3 Direction { get; set; }
        [Networked] private int MainActionCount { get; set; }

        private GameObject _playerUnitObject;
        private int _preMainActionCount = 0;

        public override void Spawned()
        {
            InstantiatePlane();
            _preMainActionCount = MainActionCount;
        }

        void InstantiateTank()
        {
            
        }

        void InstantiatePlane()
        {
            DestroyPlayerUnit();
            _playerUnitObject = Instantiate(planePrefab, transform);
            var animator = _playerUnitObject.GetComponentInChildren<Animator>();
            _animatorSetter = new PlaneAnimatorSetter(new PlaneAnimatorSetterInfo()
            {
                Animator = animator
            });
            _animatorSetter.OnSpawn();
        }

        void DestroyPlayerUnit()
        {
            if (_playerUnitObject != null)
            {
                Destroy(_playerUnitObject);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    MainActionCount++;
                }

                Direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                PreButtons = input.Buttons;
            }
        }

        public override void Render()
        {
            _animatorSetter.OnMove(Direction);

            if (MainActionCount > _preMainActionCount)
            {
                _animatorSetter.OnMainAction();
                _preMainActionCount = MainActionCount;
            }
        }
    }

}