using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public class NetworkBoss1Controller : PoolableObject
    {
        // Serialize Record
        [SerializeField] Boss1Record _record;
        [SerializeField] GameObject _modelPrefab; // ToDo: インスペクタで設定する作りはよくない ロードする作りに変える

        // INetworkStruct
        ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();

        // Networked TickerTimer
        //Timer
        [Networked] TickTimer AttackTimer { get; set; }
        [Networked] TickTimer SetAsWillStateTimer { get; set; }

        // Stats
        [Networked] int Hp { get; set; } = 1;

        // Domain
        IBoss1Context _context;


        // Decoration
        Boss1DecorationDetector _decorationDetector;

        public override void Spawned()
        {
            Init();
        }

        void Init()
        {
            // Init record.
            _record = new Boss1Record(Runner, gameObject);

            // Instantiate the boss.
            InstantiateBoss();
        }
        
        protected override void OnInactive()
        {
            //　ファイナライザ的な処理を書く
        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);
        }
    }
}