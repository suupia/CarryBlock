﻿using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity , IHighlightExecutor
    {
        IBlock? Block { get; }
        IList<IBlock> Blocks { get; }
    }
}