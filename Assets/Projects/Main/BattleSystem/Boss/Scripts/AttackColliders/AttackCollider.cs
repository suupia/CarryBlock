using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider))]
public abstract class AttackCollider : MonoBehaviour
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

// 以下はUnit側のインターフェース
public interface IPlayerOnAttacked
{
    NetworkPlayerStruct NetworkPlayerStruct { get; }
    
    /// <summary>
    /// 外部からNetworkPlayerStructを受け取り被ダメージの処理をする
    /// </summary>
    /// <param name="networkPlayerStruct"></param>
    void OnAttacked(NetworkPlayerStruct networkPlayerStruct);
}

