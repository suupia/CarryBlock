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
        [SerializeField] NetworkPlayerUnit[] playerUnits;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] NetworkBool IsReady { get; set; }
        [Networked] NetworkPlayerUnit NowUnit { get; set; }

        public override void Spawned()
        {
            //Spawn init player unit
            if (Object.HasStateAuthority)
            {
                var prefab = playerUnits[0];
                var position = new Vector3(0, 1, 0);
                var rotation = Quaternion.identity;

                NowUnit = Runner.Spawn(prefab, position, rotation, Runner.LocalPlayer);
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
                
                var direction = new Vector3(input.horizontal, 0, input.vertical).normalized;

                //Apply input
                NowUnit.Move(direction);
                NowUnit.Action(input.buttons, PreButtons);

                PreButtons = input.buttons;
            }
        }


    }
}

