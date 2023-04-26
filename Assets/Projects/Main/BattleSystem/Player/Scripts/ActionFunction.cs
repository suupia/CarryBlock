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
        readonly NetworkRunner _runner;
        readonly GameObject _playerObj;
        readonly PlayerInfoForPicker _info;
        readonly PrefabLoaderFromResources<NetworkPickerController> _prefabLoaderFromResources;

        readonly float _pickerHeight = 5.0f;

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
        readonly NetworkRunner _runner;
        readonly GameObject _playerObj;

        readonly float _collectTime = 1f;
        readonly float _collectOffset = 0.5f; // determine how much to place the resource below.
        readonly float _detectionRange = 3f;
        readonly float _submitResourceRange = 3f;

        bool _isCollecting;
        IList<NetworkObject> _heldResources = new List<NetworkObject>();

        public CollectResource(NetworkRunner runner, GameObject playerObj)
        {
            _runner = runner;
            _playerObj = playerObj;
        }

        public float ActionCooldown() => 0.1f;

        public bool InAction() => _isCollecting;

        public void Action()
        {
            // Collect resource.
            if (_heldResources.Any())
            {
                SubmitResource();
            }
            else
            {
                var _ = AttemptCollectResource();
            }
        }

        bool AttemptCollectResource()
        {
            Collider[] colliders =
                Physics.OverlapSphere(Utility.SetYToZero(_playerObj.transform.position), _detectionRange);
            Debug.Log($"colliders.Length = {colliders.Length}, colliders = {colliders}");
            var resources = colliders.Where(collider => collider.CompareTag("Resource"))
                .Where(collider => collider.gameObject.GetComponent<NetworkResourceController>().canAccess)
                .Select(collider => collider.gameObject.GetComponent<NetworkObject>());
            if (resources.Any())
            {
                CollectResourceAction(resources.First());
                return true;
            }
            else
            {
                return false;
            }
        }

        async void CollectResourceAction(NetworkObject resource)
        {
            if (resource == null) return;
            if (_isCollecting) return;

            var initPos = _playerObj.transform.position;
            var deltaVector = resource.transform.position - initPos;

            _isCollecting = true;

            for (float t = 0; t < _collectTime; t += Time.deltaTime)
            {
                var coefficient = 2 * Mathf.PI / _collectTime;
                var progress = -Mathf.Cos(coefficient * t) + 1f;

                _playerObj.transform.position = progress * deltaVector + initPos;

                await UniTask.Yield();
            }

            Debug.Log("complete collect");
            resource.transform.position = _playerObj.transform.position - new Vector3(0, _collectOffset, 0);
            resource.GetComponent<NetworkResourceController>().OnHeld(_playerObj.transform);
            _heldResources.Add(resource);
            _isCollecting = false;
        }

        void SubmitResource()
        {
            if (!_heldResources.Any()) return;
            if (!IsNearMainBase()) return;

            foreach (var resource in _heldResources)
            {
                _runner.Despawn(resource);
                Debug.Log($"submit resource");
            }

            _heldResources = new List<NetworkObject>();
        }

        bool IsNearMainBase()
        {
            Collider[] colliders =
                Physics.OverlapSphere(Utility.SetYToZero(_playerObj.transform.position), _submitResourceRange);
            var mainBases = colliders.Where(collider => collider.CompareTag("MainBase"))
                .Select(collider => collider.gameObject);
            Debug.Log($"IsNearMainBase():{mainBases.Any()}");
            return mainBases.Any();
        }
    }
}