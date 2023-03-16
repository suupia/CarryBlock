using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class PlayerSpawner
{
    GameObject playerPrefab;

    PlayerController.UnitType unitType;

    public PlayerSpawner()
    {
        playerPrefab  = Resources.Load<GameObject>("Prefabs/Player");
        Debug.Log($"playerPrefab:{playerPrefab}");
    }

    public void SpawnPlayer()
    {
        var playerController = Object.Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Initialize(unitType);
    }

    public void SetUnitType(PlayerController.UnitType unitType)
    {
        this.unitType = unitType;
    }


}
