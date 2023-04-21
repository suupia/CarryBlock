using System;
using UnityEngine;

namespace Exp
{
    /// <summary>
    /// 経験値を管理するクラス。使用するかどうかは検討
    /// ヴァンパイアサバイバーズの経験値ゲージのようなものを目指した。
    ///
    /// 現状、必要経験値が等比数列的に上がるようにしている。
    /// 将来的には抽象化して、いろいろな関数を取り入れられるようにする。
    /// （使用されるかわからない段階で過度な抽象化はしない）
    /// 
    /// 基本的にはPlayerControllerが（間接的か直接的か問わず）保持するクラス
    /// （オブジェクト指向の Has a の考えから。）
    /// その上で、適切なタイミングでAddを呼んでもらう
    /// Addはボスが倒されたときに呼ばれるべきである。以下の戦略が考えられる
    ///   - Playerは間接的に攻撃をするので、間接的な物体にPlayerの識別子や参照を持たせる
    ///     その上で、ボスのOnColliderなどの関数で、致死の際にAddを呼び出す
    ///     （これは敵がPlayerに経験値を与えるようなメッセージ方式）
    ///
    /// また、関数型を意識して、定数を増やすような実装方針を取っている
    /// 具体的には、Levelなどはいちいち計算している。O(N)なので、計算量は考えない。
    /// </summary>
    public class ExpContainer
    {
        readonly private float _increaseRate;
        readonly private int _initialThreshold;
        private int _exp;
        private int _preLevel;

        private Action<int, int> _onLevelChanged = (_, _) => { };

        public int Exp => _exp;

        //TODO: add unit test
        public int ThresholdToNextLevel => ThresholdExpTo(_preLevel + 1);
        public int RequiresExpToNextLevel => ThresholdToNextLevel - ThresholdExpTo(_preLevel);
        public int RemainingExpToNextLevel => ThresholdToNextLevel - Exp;
        public float RateToNextLevel => 1 - (float)RemainingExpToNextLevel / RequiresExpToNextLevel;

        public int Level {
            get
            {
                var level = 1;

                while (_exp >= ThresholdExpTo(level)) level++;

                return level;
            }
        }


        public ExpContainer(int initialExp = 0, int initialThreshold = 100, float increaseRate = 1.1f)
        {
            _exp = initialExp;
            _increaseRate = increaseRate;
            _initialThreshold = initialThreshold;
            _preLevel = 1;
        }

        /// <summary>
        /// 等比数列の和を返す。公式もあるが、可読性と拡張性を考えて、ループで記述中
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int ThresholdExpTo(int level)
        {
            var l = 1;
            var threshold = _initialThreshold;
            while (l < level)
            {
                threshold += (int)(_initialThreshold * Mathf.Pow(_increaseRate, l));
                l++;
            }

            return threshold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>現在のレベル</returns>
        public int Add(int exp)
        {
            _exp += exp;

            var level = Level;
            if (_preLevel != level)
            {
                Trigger();
                _preLevel = level;
            }

            return level;
        }

        /// <summary>
        /// レベルが変わったときのコールバックを登録する関数
        /// 登録時は即座に現状のレベルが呼ばれる。この動作を変えたい場合は第二引数にfalseを指定する
        /// 第一引数は前のレベル。第二引数は現在のレベルの関数を登録できる
        /// </summary>
        /// <param name="callback"></param>
        public void Register(Action<int, int> callback, bool callImmediately = true)
        {
            _onLevelChanged += callback;
            if (callImmediately)
            {
                _onLevelChanged(_preLevel, Level);
            }
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