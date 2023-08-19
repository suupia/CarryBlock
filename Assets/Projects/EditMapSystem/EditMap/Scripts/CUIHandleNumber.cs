using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public  class CUIHandleNumber
    {
        readonly int _maxDigit = 10; // インデックスの最大の桁数
        
        public int HandleNumberInput(int index)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    if(index.ToString().Length < _maxDigit) index = index * 10 + i;
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                index = index / 10;
            }

            return index;
        }
    }
}