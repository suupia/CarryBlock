using Projects.NetworkUtility.ObjectPool.Scripts;
using UnityEngine;

namespace Projects.BattleSystem.Enemy.Scripts
{
    [RequireComponent(typeof(Collider))]
    public abstract class  AttackCollider : MonoBehaviour
    {
        // 攻撃の処理（ダメージ計算、ダメージ間隔）などのロジックを持つ
        // ホスト上でインスタンス化されるプレハブにアタッチする
        // 今のところは、gameobjectのactiveを切り替えて制御する予定
        // このコンポーネントのみをenable = falseにしたり
        // 内部のロジックでon/offを切り替えることは想定していない
    
        // インターフェースの役割を担う
        // すなわち、クライアントコードでは具体的なクラス名が出てこないようにし、AttackColliderに依存するようにする
    
        // Init()を追加して引数を渡して初期化できるようにするかも
        // ただしその時は、サブクラス全てに共通する引数にしたい。
    }

    [RequireComponent(typeof(Collider))]
    public abstract class NetworkAttackCollider : PoolableObject
    {
        // 発射物,粉塵攻撃など、NetworkBehaviourをSpawnするような見た目も伴う攻撃はこちらを使用する
    }

    [RequireComponent(typeof(Collider))]
    public abstract class NetworkTargetAttackCollider : PoolableObject
    {
        // targetが必要な攻撃はこちらを使用する
        public abstract void Init(Transform target);
    }

    /// <summary>
    /// 攻撃する側がGetComponetで取得するために必要なインターフェース
    /// </summary>
    public interface IPlayerOnAttacked
    {
        /// <summary>
        /// 外部からdamageを与えて被ダメージの処理をする
        /// 実際の処理はIUnitStatsが担う
        /// </summary>
        /// <param name="damage"></param>
        void OnAttacked(int damage);
    }

}


