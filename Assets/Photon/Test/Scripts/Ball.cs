using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }

    public void Init()
    {
        //５秒後のTickTimerを取得できる
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
            Runner.Despawn(Object);
        else
            //Interpolation Data SourceをPredictedにすると、ローカルで予測してくれる
            //予測はサーバーのスナップショットから数回分程度で、プレイヤーの入力がない場合はある程度正確
            //これによって、低遅延を装っている
            transform.position += 5 * Runner.DeltaTime * transform.forward;
    }
}
