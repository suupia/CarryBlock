using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] NetworkPrefabRef picker;

    NetworkCharacterControllerPrototype cc;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] NetworkBool IsReady { get; set; }

    public override void Spawned()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {

            Move(input);
            CheckButtons(input);

            PreButtons = input.buttons;
        }
    }
    void Move(NetworkInputData input)
    {
        var direction = new Vector3(input.horizontal, 0, input.vertical).normalized;
        cc.Move(direction);
    }

    void CheckButtons(NetworkInputData input)
    {
        if (input.buttons.GetPressed(PreButtons).IsSet(PlayerOperation.Ready))
        {
            IsReady = !IsReady;
            Debug.Log($"Toggled Ready -> {IsReady}");
        }

        if (input.buttons.GetPressed(PreButtons).IsSet(PlayerOperation.MainAction))
        {
            Runner.Spawn(picker, transform.position, transform.rotation, PlayerRef.None);
        }
    }

}
