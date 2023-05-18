namespace Main
{
    public interface IEnemyOnAttacked
    {
        /// <summary>
        /// 外部からdamageを与えて被ダメージの処理をする
        /// 実際の処理はIUnitStatsが担う
        /// </summary>
        /// <param name="damage"></param>
        void OnAttacked(int damage);
    }
}