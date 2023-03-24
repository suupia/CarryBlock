using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Tank : NetworkPlayerUnit
{
    [SerializeField] NetworkPrefabRef picker;

    NetworkCharacterControllerPrototype cc;

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void Move(Vector3 direction)
    {
        cc.Move(direction);

    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        if (buttons.GetPressed(preButtons).IsSet(PlayerOperation.MainAction))
        {
            Runner.Spawn(picker, transform.position, transform.rotation, PlayerRef.None);
        }
    }
}
