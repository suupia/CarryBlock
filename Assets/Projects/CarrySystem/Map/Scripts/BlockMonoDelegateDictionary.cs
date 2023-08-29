using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// IBlockからIBlockMonoDelegateを取得するためのコンテナクラス
    /// </summary>
    public class BlockMonoDelegateDictionary
    {
        readonly IDictionary<IBlock, IBlockMonoDelegate> _dictionary = new Dictionary<IBlock, IBlockMonoDelegate>();
        
        public IBlockMonoDelegate GetBlockMonoDelegate (IBlock block)
        {
            if (_dictionary.TryGetValue(block, out var blockMonoDelegate))
            {
                return blockMonoDelegate;
            }
            else
            {
                Debug.LogError($"BlockMonoDelegateDictionaryに{block}が登録されていません");
                return null!;
            }
        }
        public void Add(IBlock block, IBlockMonoDelegate blockMonoDelegate)
        {
            _dictionary.Add(block, blockMonoDelegate);
        }
    }
}