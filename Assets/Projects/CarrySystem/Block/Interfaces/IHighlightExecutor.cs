#nullable enable
using Fusion;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IHighlightExecutor
    {
        void Highlight(IBlock? block, PlayerRef playerRef );
    }
}