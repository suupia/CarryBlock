using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkManager
{

    public void SetActiveLobbyScene()
    {
        if (Object.HasStateAuthority)
        {
            phaseManager.SetPhase(Phase.Ending);
        }
    }
}
