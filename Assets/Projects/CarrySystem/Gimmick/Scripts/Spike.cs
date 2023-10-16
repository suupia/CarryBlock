using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UniRx;
using UnityEngine;

namespace Projects.CarrySystem.Gimmick.Scripts
{
    [Serializable]
    public record SpikeGimmickRecord
    {
        public Spike.Kind[] kinds = new Spike.Kind[10];
    }
    
    public class Spike :  IGimmick
    {

        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 1;
        public Spike.Kind KindValue { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }
        
        public Spike(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        readonly float _spikeRepeatSpan = 10f;
        readonly float _mutekiTime = 10f;
        float _spikeOutTimer;
        float _spikeInTimer;
        float _mutekiTimer;
        bool _spikeIsOut = false;
        bool _muteki = false;
        
        
        /*別のスクリプトを作ってそれに下のを書くべき?
         CannonBallControllerNetみたいに*/
        
        // public void Update()
        // {
        //     _spikeOutTimer += Time.deltaTime;
        //     _spikeInTimer += Time.deltaTime;
        //     if (_spikeOutTimer > _spikeRepeatSpan)
        //     {
        //         _spikeOutTimer = 0f;
        //     }
        //
        //     if (_muteki)
        //     {
        //         _mutekiTimer += Time.deltaTime;
        //         if (_mutekiTimer > _mutekiTime)
        //         {
        //             _mutekiTimer = 0f;
        //             _muteki = false;
        //         }
        //     }
        // }
        //
        // void OnTriggerEnter(Collider other)
        // {
        //     if (_spikeIsOut || !_muteki)
        //     {
        //         if (other.CompareTag("Player"))
        //         {
        //             Debug.Log($"痛ったぁぁぁぁい");
        //             var playerController = other.GetComponent<AbstractNetworkPlayerController>();
        //             var character = playerController.GetCharacter;
        //             character.OnDamage();
        //             _muteki = true;
        //         }
        //         
        //     }
        // }
        
        public void StartGimmick(NetworkRunner runner)
        {
            Debug.Log("StartGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
        }
        
        public void EndGimmick(NetworkRunner runner)
        {
            Debug.Log("EndGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
        }
    }
}