using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Carry.Utility.Scripts
{
    public static class InputActionMapLoader
    {
        // NetworkRunnerManagerにDIするのがよいが、NetworkRunnerManagerがVContainerに依存するのがいやなので、
        // InputActionMapLoaderの方をstaticにして、別のところでも使用できるようにする
        public static InputActionMap GetInputActionMap(ActionMapName actionMapName)
        {
            if (!InputActionMap.ContainsKey(actionMapName) || InputActionMap[actionMapName] == null)
            {
                Load(actionMapName);
            }

            var actionMap = InputActionMap[actionMapName];
            if (actionMap == null)
            {
                Debug.LogError($"InputActionMap[{actionMapName}]がnullです");
                return null;
            }
            
            return actionMap;  
        }
        
        public enum ActionMapName
        {
            Default,
            UI
        }
        
        static readonly Dictionary<ActionMapName,InputActionMap?> InputActionMap = new ();
        

        static void Load(ActionMapName actionMapName)
        {
            var loader =
                new ScriptableObjectLoaderFromAddressable<InputActionAsset>("InputActionAssets/PlayerInputAction");
            
            (var inputActionAsset ,var handler) = loader.Load();
            Assert.IsNotNull(inputActionAsset, "InputActionを設定してください。Pathが間違っている可能性があります");

            InputActionMap.Add(actionMapName, inputActionAsset.FindActionMap(actionMapName.ToString()));
            Assert.IsNotNull(InputActionMap[actionMapName], "FindActionMap()の引数が間違っている可能性があります");

            loader.Release(handler);
        }

    }
}