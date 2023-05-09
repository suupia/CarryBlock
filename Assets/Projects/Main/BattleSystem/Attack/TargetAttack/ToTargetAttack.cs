using UnityEngine;

namespace Main
{
    /// <summary>
    /// attackで指定した攻撃を行う
    /// Targetで指定された方向を向く
    /// 
    /// IAttackをITargetAttackとして使いたいときにも使用できる
    /// 具体的には、Targetを指定したとき、ネストしたすべてのITargetAttackにTargetがセットされる
    /// </summary>
    public class ToTargetAttack : AttackWrapper, ITargetAttack
    {
        private GameObject _gameObject;
        private Transform _target;
        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                SetTarget(this);
            }
        }

        public ToTargetAttack(GameObject gameObject, IEnemyAttack attack, Transform target = null)
        {
            _target = target;
            _attack = attack;
            _gameObject = gameObject;
        }

        public void Attack()
        {
            if (Target == null) return;
            _gameObject.transform.LookAt(Target);
            _attack.Attack();
        }

        private void SetTarget(AttackWrapper wrapper)
        {
            //Targetを設定する必要があるのは、attackがITargetAttackのときのみ
            if (wrapper.attack is ITargetAttack targetAttack)
            {
                targetAttack.Target = Target;
            }
            //ラップしているattackがAttackWrapperであるなら再帰的にTargetを設定する
            if (wrapper.attack is AttackWrapper attackWrapper)
            {
                SetTarget(attackWrapper);
            }
        }
    }
}