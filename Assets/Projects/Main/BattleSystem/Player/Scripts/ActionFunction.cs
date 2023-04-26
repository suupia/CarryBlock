using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Main
{
    public interface IUnitAction
    {
        void Action();
        bool InAction();
        float ActionCooldown();
    }

    public class EjectPicker : IUnitAction
    {
        NetworkRunner _runner;
        GameObject _playerObj;
        PlayerInfoForPicker _info;
        PrefabLoaderFromResources<NetworkPickerController> _prefabLoaderFromResources;

        float _pickerHeight = 5.0f;

        public EjectPicker(NetworkRunner runner, GameObject playerObj, PlayerInfoForPicker playerInfoForPicker)
        {
            _runner = runner;
            _playerObj = playerObj;
            _info = playerInfoForPicker;
            _prefabLoaderFromResources = new PrefabLoaderFromResources<NetworkPickerController>("Prefabs/Players");
        }

        public float ActionCooldown() => 0.1f;

        public bool InAction() => false;

        public void Action()
        {
            Debug.Log($"Action()");
            var pickerPos = _playerObj.transform.position + new Vector3(0, _pickerHeight, 0);
            Debug.Log(
                $"_runner = {_runner}, _info.pickerPrefab = {_prefabLoaderFromResources.Load("Picker")}, pickerPos = {pickerPos}, PlayerRef.None = {PlayerRef.None}");
            var picker = _runner.Spawn(_prefabLoaderFromResources.Load("Picker"), pickerPos, Quaternion.identity,
                PlayerRef.None);
            Debug.Log($"picker = {picker}");
            picker.Init(_runner, _playerObj, _info);
        }
    }

    public class CollectResource : IUnitAction
    {
        NetworkRunner _runner;
        GameObject _playerObj;

        bool isCollecting;
        float collectTime = 1f;
        float collectOffset = 0.5f; // determine how much to place the resource below.
        float detectionRange = 3f;
        float submitResourceRange = 3f;
        IList<NetworkObject> heldResources = new List<NetworkObject>();

        public CollectResource(NetworkRunner runner, GameObject playerObj)
        {
            _runner = runner;
            _playerObj = playerObj;
        }

        public float ActionCooldown() => 0.1f;

        public bool InAction() => isCollecting;

        public void Action()
        {
            // Collect resource.
            if (heldResources.Any())
            {
                SubmitResource();
            }
            else
            {
               var _ = AttemptCollectResource();
            }
        }

        public bool AttemptCollectResource()
        {
            Collider[] colliders =
                Physics.OverlapSphere(Utility.SetYToZero(_playerObj.transform.position), detectionRange);
            Debug.Log($"colliders.Length = {colliders.Length}, colliders = {colliders}");
            var resources = colliders.Where(collider => collider.CompareTag("Resource"))
                .Where(collider => collider.gameObject.GetComponent<NetworkResourceController>().canAccess)
                .Select(collider => collider.gameObject.GetComponent<NetworkObject>());
            if (resources.Any())
            {
                CollectResourceAction(resources.First());
                return true;
            }else{
                return false;
            }
        }

        async void CollectResourceAction(NetworkObject resource)
        {
            if (resource == null) return;
            if (isCollecting) return;

            var initPos = _playerObj.transform.position;
            var deltaVector = resource.transform.position - initPos;

            isCollecting = true;

            for (float t = 0; t < collectTime; t += Time.deltaTime)
            {
                var coefficient = 2 * Mathf.PI / collectTime;
                var progress = -Mathf.Cos(coefficient * t) + 1f;

                _playerObj.transform.position = progress * deltaVector + initPos;

                await UniTask.Yield();
            }

            Debug.Log("complete collect");
            resource.transform.position = _playerObj.transform.position - new Vector3(0, collectOffset, 0);
            resource.GetComponent<NetworkResourceController>().OnHeld(_playerObj.transform);
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
                Physics.OverlapSphere(Utility.SetYToZero(_playerObj.transform.position), submitResourceRange);
            var mainBases = colliders.Where(collider => collider.CompareTag("MainBase"))
                .Select(collider => collider.gameObject);
            Debug.Log($"IsNearMainBase():{mainBases.Any()}");
            return mainBases.Any();
        }
    }
}