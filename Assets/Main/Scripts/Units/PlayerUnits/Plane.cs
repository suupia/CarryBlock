using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Main
{
    public class Plane : IPlayerUnit
{
    readonly NetworkRunner　_runner;
    PlayerInfo _info;
    
    bool isCollecting;
    float collectTime = 1f;
    float collectOffset = 0.5f; // determine how much to place the resource below.
    float detectionRange = 3f;

    float submitResourceRange = 3f;

    IList<NetworkObject> heldResources = new List<NetworkObject>();
    IPlayerUnitMove _move;

    public Plane(PlayerInfo info) 
    {
        _info = info;
        _runner = info._runner;
        _move = new RegularMove()
        {
            transform = _info.playerObj.transform,
            rd = _info.playerRd,
            acceleration = _info.acceleration,
            maxVelocity = _info.maxVelocity,
            maxAngularVelocity = _info.maxAngularVelocity,
            torque = _info.torque
        };
        _info.playerRd.useGravity = false;
    }

    public void Move(Vector3 direction)
    {
        if(isCollecting)return;
        
        _move.Move(direction);
    }
    
    public float ActionCooldown() => 0.1f;
    
    public void Action()
    {
        // Collect resource.
        if (heldResources.Any())
        {
            SubmitResource();
        }
        else
        {
            AttemptCollectResource();
        }
    }

    public void AttemptCollectResource()
    {
        Collider[] colliders =
            Physics.OverlapSphere(Utility.SetYToZero(_info.playerObj.transform.position), detectionRange);
        Debug.Log($"colliders.Length = {colliders.Length}, colliders = {colliders}");
        var resources = colliders.Where(collider => collider.CompareTag("Resource"))
            .Where(collider => collider.gameObject.GetComponent<NetworkResourceController>().canAccess)
            .Select(collider => collider.gameObject.GetComponent<NetworkObject>());
        if (resources.Any()) CollectResource(resources.First());
    }

    async void CollectResource(NetworkObject resource)
    {
        if (resource == null) return;
        if (isCollecting) return;

        var initPos = _info.playerObj.transform.position;
        var deltaVector = resource.transform.position - initPos;

        isCollecting = true;

        for (float t = 0; t < collectTime; t += Time.deltaTime)
        {
            var coefficient = 2 * Mathf.PI / collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;

            _info.playerObj.transform.position = progress * deltaVector + initPos;

            await UniTask.Yield();
        }

        Debug.Log("complete collect");
        resource.transform.position = _info.playerObj.transform.position - new Vector3(0, collectOffset, 0);
        resource.GetComponent<NetworkResourceController>().OnHeld(_info.playerObj.transform);
        heldResources.Add(resource);
        isCollecting = false;
    }

    void SubmitResource()
    {
        if (!heldResources.Any()) return;
        if (!IsNearMainBase()) return;

        foreach (var resource in heldResources)
        {
            _runner.Despawn(resource);
            Debug.Log($"submit resource");
        }

        heldResources = new List<NetworkObject>();
    }

    bool IsNearMainBase()
    {
        Collider[] colliders =
            Physics.OverlapSphere(Utility.SetYToZero(_info.playerObj.transform.position), submitResourceRange);
        var mainBases = colliders.Where(collider => collider.CompareTag("MainBase"))
            .Select(collider => collider.gameObject);
        Debug.Log($"IsNearMainBase():{mainBases.Any()}");
        return mainBases.Any();
    }
    
}


}

