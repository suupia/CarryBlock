using UnityEngine;

namespace Carry.UISystem.UI.TitleScene
{
    public class ResolutionChanger : MonoBehaviour
    {
        readonly float _aspectRatio = 16.0f / 9.0f;
        
        public void ChangeResolutionByWidth(int width)
        {
            //画面比に合わせて高さを計算
            int height = (int)(width / _aspectRatio);

            bool isFullScreen = Screen.fullScreen;
            
            Screen.SetResolution(width, height, isFullScreen);
        }
    }
}