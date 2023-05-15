using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class AttackCollider : MonoBehaviour
{
    // 攻撃の処理（ダメージ計算、ダメージ間隔）などのロジックを持つ
    // ホスト上でインスタンス化されるプレハブにアタッチする
    // 今のところは、gameobjectのactiveを切り替えて制御する予定
    // このコンポーネントのみをenable = falseにしたり
    // 内部のロジックでon/offを切り替えることは想定していない

    public abstract float Radius { get; set; }
}
