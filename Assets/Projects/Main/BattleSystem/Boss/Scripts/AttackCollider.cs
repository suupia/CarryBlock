using System.Collections;
using System.Collections.Generic;
using Fusion;
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
}

[RequireComponent(typeof(Collider))]
public abstract class NetworkAttackCollider : NetworkBehaviour
{
    // 発射物,粉塵攻撃など、NetworkBehaviourをSpawnするような見た目も伴う攻撃はこちらを使用する
}

public abstract class NetworkTargetAttackCollider : NetworkBehaviour
{
    // targetが必要な攻撃はこちらを使用する
    public abstract void Init(Transform target);
}