using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using System.Collections.Generic;
#nullable enable

namespace Carry.Utility.Scripts
{
    public static class InputActionMapLoader
    {
        public enum ActionMapName
        {
            Default,
            UI
        }
        
        // NetworkRunnerManagerにDIするのがよいが、NetworkRunnerManagerがVContainerに依存するのがいやなので、
        // InputActionMapLoaderの方をstaticにして、別のところでも使用できるようにする
        public static InputActionMap GetInputActionMap(ActionMapName actionMapName)
        {
            if (!_inputActionMap.ContainsKey(actionMapName) || _inputActionMap[actionMapName] == null)
            {
                Load(actionMapName);
            }
            
            return _inputActionMap[actionMapName];  
        }

        static readonly Dictionary<ActionMapName,InputActionMap?> _inputActionMap = new Dictionary<ActionMapName, InputActionMap?>();
        

        static void Load(ActionMapName actionMapName)
        {
            var loader =
                new ScriptableObjectLoaderFromAddressable<InputActionAsset>("InputActionAssets/PlayerInputAction");
            
            (var inputActionAsset ,var handler) = loader.Load();
            Assert.IsNotNull(inputActionAsset, "InputActionを設定してください。Pathが間違っている可能性があります");

            _inputActionMap.Add(actionMapName, inputActionAsset.FindActionMap(actionMapName.ToString()));
            Assert.IsNotNull(_inputActionMap[actionMapName], "FindActionMap()の引数が間違っている可能性があります");

            loader.Release(handler);
        }

    }
}