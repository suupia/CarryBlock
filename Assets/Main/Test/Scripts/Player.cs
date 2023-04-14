using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] Ball ballPrefab;

    NetworkCharacterControllerPrototype cc;
    Vector3 forward;
    Material m_Material;
    TMP_Text messages;

    [Networked] TickTimer delay { get; set; }
    [Networked(OnChanged = nameof(OnBallSpawned))]

    public NetworkBool spawned { get; set; }

    Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = GetComponentInChildren<MeshRenderer>().material;
            }
            return m_Material;

        }
    }

    public override void Render()
    {
        material.color = Color.Lerp(material.color, Color.blue, Time.deltaTime);
    }

    public static void OnBallSpawned(Changed<Player> changed)
    {
        changed.Behaviour.material.color = Color.white;
    }

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();
        messages = FindObjectOfType<TMP_Text>();
        forward = transform.forward;
    }

    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SendMessage(string message, RpcInfo info = default)
    {
        messages.text += $"{(info.IsInvokeLocal ? "You said" : "Some other player said")}: {message}\n";
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out TestNetworkInputData data))
        {
            //チートの防止などの理由から正規化する
            Vector3 direction = new();

            if (data.buttons.IsSet(TestPlayerOperations.Forward))
                direction += Vector3.forward;

            if (data.buttons.IsSet(TestPlayerOperations.Backward))
                direction += Vector3.back;

            if (data.buttons.IsSet(TestPlayerOperations.Left))
                direction += Vector3.left;

            if (data.buttons.IsSet(TestPlayerOperations.Right))
                direction += Vector3.right;

            //ネットワーク上のDeltaTimeを考慮する。Time.deltaTimeの代わり
            //逆にTime.deltaTimeは使ってはいけない
            cc.Move(5 * Runner.DeltaTime * direction);

            if (direction.sqrMagnitude > 0 )
                forward = direction;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                if (data.buttons.IsSet(TestPlayerOperations.Attack))
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
                    spawned = !spawned;
                }
            }

        }
    }
}
