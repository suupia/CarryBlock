using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUnit
{
    void Move(Vector3 direction);
    void Action(NetworkButtons buttons, NetworkButtons preButtons);
}

public class NetworkPlayerUnit : NetworkBehaviour, IPlayerUnit
{

    public virtual void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
