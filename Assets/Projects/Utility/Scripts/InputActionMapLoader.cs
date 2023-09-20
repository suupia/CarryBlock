using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Projects.Utility.Scripts
{
    public class InputActionMapLoader
    {
        public InputActionMap InputActionMap { get; private set; }
        public  InputActionMapLoader()
        {
            Load();
        }

        void Load()
        {
            var loader =
                new ScriptableObjectLoaderFromAddressable<InputActionAsset>("InputActionAssets/PlayerInputAction");
            
            (var inputActionAsset ,var handler) = loader.Load();
            Assert.IsNotNull(inputActionAsset, "InputActionを設定してください。Pathが間違っている可能性があります");

            InputActionMap = inputActionAsset.FindActionMap("Default");
            // InputActionMap.Enable();

            loader.Release(handler);
        }

    }
}