using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] Ball ballPrefab;

    NetworkCharacterControllerPrototype cc;
    Vector3 forward;

    [Networked] TickTimer delay { get; set; }

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();
        forward = transform.forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //チートの防止などの理由から正規化する
            data.direction.Normalize();

            //ネットワーク上のDeltaTimeを考慮する。Time.deltaTimeの代わり
            //逆にTime.deltaTimeは使ってはいけない
            cc.Move(5 * Runner.DeltaTime * data.direction);

            if (data.direction.sqrMagnitude > 0 )
                forward = data.direction;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) == 1)
                {
                    //頻度を制限
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(
                        ballPrefab, 
                        transform.position + forward, 
                        Quaternion.LookRotation(forward), 
                        Object.InputAuthority,
                        (runner, obj) =>
                        {
                            obj.GetComponent<Ball>().Init();
                        }
                    );
                }
            }

        }
    }
}
