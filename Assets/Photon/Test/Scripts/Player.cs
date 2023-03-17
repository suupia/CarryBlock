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
        if (GetInput(out NetworkInputData data))
        {
            //�`�[�g�̖h�~�Ȃǂ̗��R���琳�K������
            data.direction.Normalize();

            //�l�b�g���[�N���DeltaTime���l������BTime.deltaTime�̑���
            //�t��Time.deltaTime�͎g���Ă͂����Ȃ�
            cc.Move(5 * Runner.DeltaTime * data.direction);

            if (data.direction.sqrMagnitude > 0 )
                forward = data.direction;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) == 1)
                {
                    //�p�x�𐧌�
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
