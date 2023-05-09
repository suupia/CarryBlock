using Animations;
using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public interface IBoss1 : IMove
    {
    }
    public class NetworkBoss1Controller : PoolableObject
    {
        // Serialize Record
        [SerializeField] Boss1Record _record;
        [SerializeField] GameObject _modelPrefab; // ToDo: インスペクタで設定する作りはよくない ロードする作りに変える

        // INetworkStruct
        ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();

        // Networked TickerTimer
        // Cooldown Timer
        [Networked] TickTimer AttackCooldown { get; set; }

        // Stats
        [Networked] int Hp { get; set; } = 1;

        // Domain
        IBoss1 _boss1;
        
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


            _boss1.Move();

            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                // currentState.Attack();
            }
            
            
        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            var stateGenerator = new Boss1StateGenerator(Runner, _record);
            var context = new Boss1Context(stateGenerator.LostState);
            _boss1 = new Boss1IncludeDecorationDetector(stateGenerator, context);
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));
        }
    }

    public class Boss1IncludeDecorationDetector : IBoss1
    {
        Boss1StateGenerator _stateGenerator;
        readonly IBoss1Context _context;

        public Boss1IncludeDecorationDetector(Boss1StateGenerator stateGenerator, IBoss1Context context)
        {
            _stateGenerator = stateGenerator;
            _context = context;
        }

        public void Move(Vector3 input)
        {
            _context.CurrentState.Move(input);
        }
        // public void Search() => _context.CurrentState.Search();
        // public void Attack() => _context.CurrentState.Attack();
    }

}