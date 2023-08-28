using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    
    /// <summary>
    /// NetworkBehaviourから切り出した処理をまとめて、ICharacterを生成する
    /// </summary>
    public interface ICarryPlayerFactory
    {
        ICharacter Create(PlayerColorType colorType);
    }
}