using Fusion;
using UnityEngine;

namespace MyFusion
{
    /// <summary>
    /// Manage input, now playing unit, or something...
    /// This Controller class manages "NetworkPlayerUnit"
    /// </summary>
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] NetworkPlayerUnit[] playerUnitPrefabs;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }
        [Networked] NetworkPlayerUnit Unit { get; set; }

        public NetworkPlayerUnit NowUnit => Unit;

        public override void Spawned()
        {
            //Spawn init player unit
            if (Object.HasStateAuthority)
            {
                Unit = SpawnPlayerUnit(0);
            }
            
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Unit.Object); 
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                //TODO: Check phase
                if (input.buttons.GetPressed(PreButtons).IsSet(PlayerOperation.Ready))
                {
                    IsReady = !IsReady;
                    Debug.Log($"Toggled Ready -> {IsReady}");
                }

                if (input.buttons.GetPressed(PreButtons).IsSet(PlayerOperation.ChangeUnit))
                {
                    //Tmp
                    RPC_ChangeUnit(1);
                }
                
                var direction = new Vector3(input.horizontal, 0, input.vertical).normalized;

                //Apply input
                Unit.Move(direction);
                Unit.Action(input.buttons, PreButtons);

                PreButtons = input.buttons;
            }
        }

        //Deal as RPC for changing unit
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_ChangeUnit(int index)
        {
            if (Unit != null)
            {
                Runner.Despawn(Unit.Object);

            }
            Unit = SpawnPlayerUnit(index);
        }

        NetworkPlayerUnit SpawnPlayerUnit(int index)
        {
            var prefab = playerUnitPrefabs[index];
            var position = new Vector3(0, 1, 0);
            var rotation = Quaternion.identity;

            return Runner.Spawn(prefab, position, rotation, Runner.LocalPlayer);
        }
     }
}
