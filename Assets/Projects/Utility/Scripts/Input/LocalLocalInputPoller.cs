using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Carry.Utility.Scripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Projects.Utility.Scripts
{
    public class LocalLocalInputPoller
    {
        readonly InputAction _toggleSelectStageCanvas;
        readonly InputAction _move;
        readonly InputAction _mainAction;
        readonly InputAction _dash;
        readonly InputAction _pass;
        readonly InputAction _changeUnit;

        public LocalLocalInputPoller()
        {
            var inputActionMap = InputActionMapLoader.GetInputActionMap(InputActionMapLoader.ActionMapName.Default);

            inputActionMap.Enable();

            _toggleSelectStageCanvas = inputActionMap.FindAction("ToggleSelectStageCanvas");
            _move = inputActionMap.FindAction("Move");
            _mainAction = inputActionMap.FindAction("MainAction");
            _dash = inputActionMap.FindAction("Dash");
            _pass = inputActionMap.FindAction("Pass");
            _changeUnit = inputActionMap.FindAction("ChangeUnit");
        }

        public bool GetInput(out LocalInputData input)
        {
            input = default;

            var isDownOpenSelectStageCanvas = _toggleSelectStageCanvas.ReadValue<float>();
            var moveVector = _move.ReadValue<Vector2>().normalized;
            var mainActionValue = _mainAction.ReadValue<float>(); 
            var dashValue = _dash.ReadValue<float>();
            var passValue = _pass.ReadValue<float>();
            var changeUnitValue = _changeUnit.ReadValue<float>();

            input.Horizontal = moveVector.x;
            input.Vertical = moveVector.y;
            input.Buttons.Set(PlayerOperation.ToggleSelectStageCanvas, isDownOpenSelectStageCanvas != 0);
            input.Buttons.Set(PlayerOperation.MainAction, mainActionValue != 0);
            input.Buttons.Set(PlayerOperation.Dash, dashValue != 0);
            input.Buttons.Set(PlayerOperation.Pass, passValue != 0);
            input.Buttons.Set(PlayerOperation.ChangeUnit, changeUnitValue != 0);

            return true;  // 本来のFusionのGetInput()の動作を正しく理解していないが、とりあえずtrueを返して毎回判定するようにすればオッケー
        }
        
        
    }

    public struct LocalInputData
    {
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public LocalButtons Buttons;
        public override bool Equals(object obj)
        {
            if (obj is LocalInputData other)
            {
                // LocalInputData の比較ロジック
                return this.Horizontal == other.Horizontal &&
                       this.Vertical == other.Vertical &&
                       this.Buttons.Equals(other.Buttons);
            }
            return false;
        }

        // public override int GetHashCode()
        // {
        //     // ハッシュコードの生成
        // }

        public static bool operator ==(LocalInputData lhs, LocalInputData rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(LocalInputData lhs, LocalInputData rhs)
        {
            return !lhs.Equals(rhs);
        }

    }


    
}