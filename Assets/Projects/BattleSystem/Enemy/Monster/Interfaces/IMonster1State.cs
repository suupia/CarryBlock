namespace Projects.BattleSystem.Enemy.Monster.Interfaces
{
    
    //Stateパターン
    public interface IMonster1Context
    {
        public IMonster1State CurrentState { get; }
        public void ChangeState(IMonster1State state);
    }

    public interface IMonster1State : IEnemyMove, IEnemyAction, IEnemySearch
    {
        IEnemyMove EnemyMove { get; } 
        IEnemyAction EnemyAction { get; }
    }

}