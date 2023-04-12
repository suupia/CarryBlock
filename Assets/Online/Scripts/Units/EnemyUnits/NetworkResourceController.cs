using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkResourceController : NetworkBehaviour
{
    public  bool isOwned = false;
}
