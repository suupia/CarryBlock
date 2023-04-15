using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Plane : IPlayerUnit
{
    readonly NetworkRunner　_runner;
    PlayerInfo _info;
    NetworkCharacterControllerPrototype _cc;
    
    bool isCollecting;
    float collectTime = 1f;
    float collectOffset = 0.5f; // determine how much to place the resource below.
    float detectionRange = 3f;

    float submitResourceRange = 3f;


    IList<GameObject> heldResources = new List<GameObject>();


    
    public Plane(PlayerInfo info) 
    {
        _info = info;
        _runner = info.runner;
        _cc = info.networkCharacterController; 
    }

    public void Move(Vector3 direction)
    {
        _cc.Move(direction);
    }
    
    public float ActionCooldown() => 0.1f;
    
    public void Action()
    {
        // Collect resource.
        AttemptCollectResource();
        SubmitResource();
    }

    public void AttemptCollectResource()
    {
        Collider[] colliders =
            Physics.OverlapSphere(Utility.SetYToZero(_info.unitObject.transform.position), detectionRange);
        Debug.Log($"colliders.Length = {colliders.Length}, colliders = {colliders}");
        var resources = colliders.Where(collider => collider.CompareTag("Resource"))
            .Where(collider => collider.gameObject.GetComponent<NetworkResourceController>().isOwned == false)
            .Select(collider => collider.gameObject);
        if (resources.Any()) CollectResource(resources.First());
    }

    async void CollectResource(GameObject resource)
    {
        if (resource == null) return;
        if (isCollecting) return;

        var initPos = _info.unitObject.transform.position;
        var deltaVector = resource.transform.position - initPos;

        isCollecting = true;

        for (float t = 0; t < collectTime; t += Time.deltaTime)
        {
            var coefficient = 2 * Mathf.PI / collectTime;
            var progress = -Mathf.Cos(coefficient * t) + 1f;

            _info.unitObject.transform.position = progress * deltaVector + initPos;

            await UniTask.Yield();
        }

        Debug.Log("complete collect");
        resource.transform.position = _info.unitObject.transform.position - new Vector3(0, collectOffset, 0);
        resource.transform.parent = _info.unitObject.transform;
        heldResources.Add(resource);
        isCollecting = false;
    }

    void SubmitResource()
    {
        if (!heldResources.Any()) return;
        if (!IsNearMainBase()) return;

        foreach (var resource in heldResources)
        {
            Object.Destroy(resource);
            Debug.Log($"submit resource");
        }

        heldResources = new List<GameObject>();
    }

    bool IsNearMainBase()
    {
        Collider[] colliders =
            Physics.OverlapSphere(Utility.SetYToZero(_info.unitObject.transform.position), submitResourceRange);
        var mainBases = colliders.Where(collider => collider.CompareTag("MainBase"))
            .Select(collider => collider.gameObject);
        Debug.Log($"IsNearMainBase():{mainBases.Any()}");
        return mainBases.Any();
    }
    
}

