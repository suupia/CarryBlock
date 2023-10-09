using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Carry.Utility.Editor;
using UnityEngine.UI;

#nullable enable

namespace Carry.UISystem.UI.Prefabs
{
    /// <summary>
    /// 画像を自由に設定可能なボタン
    /// </summary>
    public class CustomViewButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image frameImage = null!;
        [SerializeField] Image iconImage = null!;
        [SerializeField] Image pressedImage = null!;
        [SerializeField] TextMeshPro text = null!;
        
        Action<object>? _clickEvent;
        object? _clickEventTag;
        
        enum ButtonState
        {
            Normal,
            Hover,
            Pressed,
        }
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            _clickEvent?.Invoke(_clickEventTag);
            DisplayButton(ButtonState.Pressed);
        }
        
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            DisplayButton(ButtonState.Hover);
        }
        
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            DisplayButton(ButtonState.Normal);
        }
        
        void DisplayButton(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Normal:
                    frameImage.color = new Color(frameImage.color.r,frameImage.color.g,frameImage.color.b,1);
                    iconImage.color = new Color(iconImage.color.r,iconImage.color.g,iconImage.color.b,1);
                    pressedImage.color = new Color(pressedImage.color.r,pressedImage.color.g,pressedImage.color.b,0);
                    text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
                
                case ButtonState.Hover:
                    frameImage.color = new Color(frameImage.color.r,frameImage.color.g,frameImage.color.b,1);
                    iconImage.color = new Color(iconImage.color.r,iconImage.color.g,iconImage.color.b,0);
                    pressedImage.color = new Color(pressedImage.color.r,pressedImage.color.g,pressedImage.color.b,0.5f);
                    text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
                
                case ButtonState.Pressed:
                    frameImage.color = new Color(frameImage.color.r,frameImage.color.g,frameImage.color.b,1);
                    iconImage.color = new Color(iconImage.color.r,iconImage.color.g,iconImage.color.b,0);
                    pressedImage.color = new Color(pressedImage.color.r,pressedImage.color.g,pressedImage.color.b,1);
                    text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
            }   
        }
    }
}