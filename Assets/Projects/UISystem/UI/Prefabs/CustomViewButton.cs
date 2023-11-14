using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable enable

namespace Carry.UISystem.UI.Prefabs
{
    /// <summary>
    /// 画像を自由に設定可能なボタン
    /// </summary>
    public class CustomViewButton : 
        MonoBehaviour, 
        IPointerClickHandler,
        IPointerEnterHandler, 
        IPointerExitHandler
    {
        [SerializeField] Image frameImage = null!;
        [SerializeField] Image iconImage = null!;
        [SerializeField] Image pressedImage = null!;
        [SerializeField] TextMeshPro? text;

        float ClickInterval { get; set; } = 0.1f;

        bool  _isClickable;
        float _clickTime;
        
        Action<object>? _clickEvent;
        object? _clickEventTag;

        void Update()
        {
            if (!_isClickable)
            {
                if (_clickTime + ClickInterval < Time.time)
                {
                    _isClickable = true;
                }
            }
        }

        enum ButtonState
        {
            Normal,
            Hover,
            Pressed,
        }
        
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!_isClickable)
            {
                Debug.Log("連打を防止するためにクリックを無視する");
                return;
            }
            
            _isClickable = false;
            _clickEvent?.Invoke(_clickEventTag);
            Debug.Log("ボタンがクリックされた(押され，ドラッグされずに離された)");
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("カーソルがボタンに重なった");
            DisplayButton(ButtonState.Hover);
        }
        
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("カーソルがボタンから離れた");
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
                    if(text != null)text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
                
                case ButtonState.Hover:
                    frameImage.color = new Color(frameImage.color.r,frameImage.color.g,frameImage.color.b,1);
                    iconImage.color = new Color(iconImage.color.r,iconImage.color.g,iconImage.color.b,0);
                    pressedImage.color = new Color(pressedImage.color.r,pressedImage.color.g,pressedImage.color.b,0.2f);
                    if(text != null)text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
                
                case ButtonState.Pressed:
                    frameImage.color = new Color(frameImage.color.r,frameImage.color.g,frameImage.color.b,1);
                    iconImage.color = new Color(iconImage.color.r,iconImage.color.g,iconImage.color.b,0);
                    pressedImage.color = new Color(pressedImage.color.r,pressedImage.color.g,pressedImage.color.b,1);
                    if(text != null)text.color = new Color(text.color.r,text.color.g,text.color.b,1);
                    break;
            }   
        }
    }
}