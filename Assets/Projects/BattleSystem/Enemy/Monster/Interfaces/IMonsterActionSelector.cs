namespace Nuts.BattleSystem.Enemy.Monster.Interfaces
{
    public interface IBoss1ActionSelector
    {
        IMonster1State SelectAction(params IMonster1State[] attacks);
    }

}