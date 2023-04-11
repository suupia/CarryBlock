using Fusion;
using System;
using System.Linq;
using UnityEngine;

public class NetworkEnemyController : NetworkBehaviour
{
    [SerializeField] NetworkPrefabRef resourcePrefab;
    
    float detectionRange = 30;

    GameObject targetPlayerObj;

    public enum EnemyState
    {
        idle,chasingPlayer,
    }

    EnemyState state = EnemyState.idle;

    NetworkCharacterControllerPrototype _cc;


    public Action OnDespawn = () => { };

    public override void Spawned()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();

    }

    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority) return;
        
        switch (state)
        {
            case EnemyState.idle:
                Search();
                break;
            case EnemyState.chasingPlayer:
                Chase();
                break;
        }
        // if (Hp <= 0)
        // {
        //     Runner.Despawn(Object);
        //     return;
        // }
        // _cc.Move(new Vector3(Direction.x, 0, Direction.y) * Speed);
    }

    void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, detectionRange);
        var players = colliders.
            Where(collider => collider.CompareTag("Player")).
            Select(collider => collider.gameObject);
        if (players.Any())
        {
            targetPlayerObj = players.First();
            state = EnemyState.chasingPlayer;
        }
        else
        {
            state = EnemyState.idle;
        }
    }

    void Chase()
    {
        var directionVec = Utility.SetYToZero(targetPlayerObj.transform.position - gameObject.transform.position).normalized;
        _cc.Move(directionVec);
    }

    public void OnDefeated()
    {
        // Instantiate(resourcePrefab,transform.position, Quaternion.identity,resourcesParent);
        // Todo: ここでリソースを生成する
        OnDespawn();
        Runner.Despawn(Object);
    }

    
    //一旦簡単なモデルで実装する
    //これはNetworkManagerによって呼ばれ、自身の進むべき方向を決める
    //変える可能性が高い
    // public void SetDirection(NetworkPlayerUnit[] playerUnits)
    // {
    //     //とりあえず、一番近いプレイヤーに向かう。やや重たい処理になるか
    //     int minIndex = 0;
    //     float min = float.MaxValue;
    //     for (int i = 0; i < playerUnits.Length; i++)
    //     {
    //         var _min = Vector3.Distance(playerUnits[i].transform.position, transform.position);
    //         if (_min < min)
    //         {
    //             min = _min;
    //             minIndex = i;
    //         }
    //     }
    //
    //     var target = playerUnits[minIndex].transform.position;
    //     var direction = target - transform.position;
    //     Direction = new Vector2(direction.x, direction.z);
    // }
    //
    // public override void Despawned(NetworkRunner runner, bool hasState)
    // {
    //     OnDespawn.Invoke();
    // }
    //
    //
    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.gameObject.CompareTag("Harmful"))
    //     {
    //         Hp -= 1;
    //     }
    // }
}
