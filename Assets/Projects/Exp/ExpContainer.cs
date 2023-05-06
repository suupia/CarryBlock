using System;
using UnityEngine;

namespace Exp
{
    /// <summary>
    ///     経験値を管理するクラス。使用するかどうかは検討
    ///     経験値ゲージのようなものを目指した。
    ///     現状、必要経験値が等比数列的に上がるようにしている。
    ///     将来的には抽象化して、いろいろな関数を取り入れられるようにする。
    ///     （使用されるかわからない段階で過度な抽象化はしない）
    ///     基本的にはPlayerControllerが（間接的か直接的か問わず）保持するクラス
    ///     （オブジェクト指向の Has a の考えから。）
    ///     その上で、適切なタイミングでAddを呼んでもらう
    ///     Addはボスが倒されたときに呼ばれるべきである。以下の戦略が考えられる
    ///     - Playerは間接的に攻撃をするので、間接的な物体にPlayerの識別子や参照を持たせる
    ///     その上で、ボスのOnColliderなどの関数で、致死の際にAddを呼び出す
    ///     （これは敵がPlayerに経験値を与えるようなメッセージ方式）
    ///     また、関数型を意識して、定数を増やすような実装方針を取っている
    ///     具体的には、Levelなどはいちいち計算している。O(N)なので、計算量は考えない。
    /// </summary>
    public class ExpContainer
    {
        private readonly float _increaseRate;
        private readonly int _initialThreshold;

        private Action<int, int> _onLevelChanged = (_, _) => { };
        private int _preLevel;


        public ExpContainer(int initialExp = 0, int initialThreshold = 100, float increaseRate = 1.1f)
        {
            Exp = initialExp;
            _preLevel = 1;
            _increaseRate = increaseRate;
            _initialThreshold = initialThreshold;
        }

        /// <summary>
        ///     現在の経験値
        /// </summary>
        public int Exp { get; private set; }

        /// <summary>
        ///     次のレベルまでのしきい値
        /// </summary>
        private int ThresholdToNextLevel => ThresholdExpTo(Level);

        /// <summary>
        ///     現在の経験値から次のレベルまでに必要な経験値
        /// </summary>
        public int RequiresExpToNextLevel => ThresholdToNextLevel - ThresholdExpTo(Level - 1);

        /// <summary>
        ///     現在の経験値から次のレベルまでに必要な残り経験値
        /// </summary>
        public int RemainingExpToNextLevel => ThresholdToNextLevel - Exp;

        /// <summary>
        ///     現在のレベル
        /// </summary>
        public int Level
        {
            get
            {
                var level = 1;

                while (Exp >= ThresholdExpTo(level)) level++;

                return level;
            }
        }

        /// <summary>
        ///     等比数列の和を返す。公式もあるが、可読性と拡張性を考えて、ループで記述中
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int ThresholdExpTo(int level)
        {
            if (level <= 0) return 0;

            var lv = 1;
            var threshold = _initialThreshold;
            while (lv < level)
            {
                threshold += (int)(_initialThreshold * Mathf.Pow(_increaseRate, lv));
                lv++;
            }

            return threshold;
        }

        /// <summary>
        ///     この関数内でのみ、副作用を許容する
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>現在のレベル</returns>
        public int Add(int exp)
        {
            Exp += exp;

            if (_preLevel != Level)
            {
                Trigger();
                _preLevel = Level;
            }

            return Level;
        }

        /// <summary>
        ///     レベルが変わったときのコールバックを登録する関数
        ///     第一引数は前のレベル、第二引数は現在のレベルの関数を登録できる
        ///     登録時は即座に現状のレベルが呼ばれる。この動作を変えたい場合は第二引数にfalseを指定する
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="callImmediately"></param>
        public void Register(Action<int, int> callback, bool callImmediately = true)
        {
            _onLevelChanged += callback;
            if (callImmediately) _onLevelChanged(_preLevel, Level);
        }

        public void Unregister(Action<int, int> callback)
        {
            _onLevelChanged -= callback;
        }

        public void Trigger()
        {
            _onLevelChanged(_preLevel, Level);
        }
    }
}