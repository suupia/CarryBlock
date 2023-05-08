using Animations;
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
        Boss1StateGenerator _stateGenerator;
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
            _record.Rd.velocity = Vector3.zero;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            _context.CurrentState.Process(_context);
        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            _stateGenerator = new Boss1StateGenerator(Runner, _record);
            _context = new Boss1Context(_stateGenerator.LostState);
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));
        }
    }

}