using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    PlayerController.UnitType unitType;

    public void SpawnPlayer()
    {
        var playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        playerController.Initialize(unitType);
    }

    public void SetUnitType(PlayerController.UnitType unitType)
    {
        this.unitType = unitType;
    }


}
