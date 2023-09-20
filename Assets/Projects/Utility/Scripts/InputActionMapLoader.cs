using log4net.Util;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
#nullable enable

namespace Projects.Utility.Scripts
{
    public static class InputActionMapLoader
    {
        // NetworkRunnerManagerにDIするのがよいが、NetworkRunnerManagerがVContainerに依存するのがいやなので、
        // InputActionMapLoaderの方をstaticにして、別のところでも使用できるようにする
        public static InputActionMap GetInputActionMap()
        {
            if (_inputActionMap == null)
            {
                Load();
            }
            
            return _inputActionMap;  
        }

        static InputActionMap? _inputActionMap;
        

        static void Load()
        {
            var loader =
                new ScriptableObjectLoaderFromAddressable<InputActionAsset>("InputActionAssets/PlayerInputAction");
            
            (var inputActionAsset ,var handler) = loader.Load();
            Assert.IsNotNull(inputActionAsset, "InputActionを設定してください。Pathが間違っている可能性があります");

            _inputActionMap = inputActionAsset.FindActionMap("Default");
            Assert.IsNotNull(_inputActionMap, "FindActionMap()の引数が間違っている可能性があります");

            loader.Release(handler);
        }

    }
}