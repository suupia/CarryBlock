using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public interface IAttack
    {
        void Attack();
    }

    public interface ITargetAttack : IAttack
    {
        Transform Target { get; set; }
    }

    public class ChooseRandomAttack : IAttack
    {
        private List<IAttack> _attacks;

        private IAttack RandomAttack =>
            _attacks[Random.Range(0, _attacks.Count)];

        public ChooseRandomAttack(params IAttack[] attacks)
        {
            if (attacks.Length == 0)
            {
                throw new ArgumentException("attacks need at least one attack");
            }

            _attacks = attacks.ToList();
        }

        public void Attack()
        {
            RandomAttack.Attack();
        }

    }

    public class ToNearestAttack : ITargetAttack
    {
        private ITargetAttack _targetAttack;
        private ISet<Transform> _targetBuffer;
        private readonly Transform _transform;


        public Transform Target
        {
            get => _targetAttack.Target;
            set => _targetAttack.Target = value;
        }

        public ToNearestAttack(Transform transform, ISet<Transform> targetBuffer, ITargetAttack targetAttack)
        {
            _transform = transform;
            _targetAttack = targetAttack;
            _targetBuffer = targetBuffer;
        }

        public void Attack()
        {
            if (_targetBuffer.Count == 0) return;

            Transform minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var transform in _targetBuffer)
            {
                var distance = Vector3.Distance(transform.position, _transform.position);
                if (distance < minDistance)
                {
                    minTransform = transform;
                    minDistance = distance;
                }
            }

            _targetAttack.Target = minTransform;
            _targetAttack.Attack();
        }
    }

    public class ToTargetAttack : ITargetAttack
    {
        private GameObject _gameObject;

        public Transform Target { get; set; }

        public ToTargetAttack(GameObject gameObject, Transform target = null)
        {
            Target = target;
            _gameObject = gameObject;
        }

        public void Attack()
        {
            if (Target == null) return;
            _gameObject.transform.LookAt(Target);
        }
    }
}