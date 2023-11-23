using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Assertions;
using DG.Tweening;
using UnityEngine.Events;

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
        public UnityAction ClickAction { get; set; } = () => {};
        
        [SerializeField] AudioClip clickSound = null!;
        [SerializeField] List<Image> buttonImage = null!;

        AudioSource? _audioSource;
        
        readonly float _clickInterval = 0.1f;

        bool  _isClickable;
        float _clickTime;

        void Start()
        {
            Assert.IsNotNull(clickSound);
            
            _audioSource = FindObjectOfType<AudioSource>();
        }

        void Update()
        {
            if (!_isClickable)
            {
                if (_clickTime + _clickInterval < Time.time)
                {
                    _isClickable = true;
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!_isClickable)
            {
                Debug.Log("連打を防止するためにクリックを無視する");
                return;
            }
            
            _isClickable = false;
            
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(clickSound);
            }
            else
            {
                Debug.LogWarning("AudioSourceが見つかりませんでした");
            }
            
            ClickAction?.Invoke();
            
            Debug.Log("ボタンがクリックされた(押され，ドラッグされずに離された)");
            
            foreach (var image in buttonImage)
            {
                image.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                {
                    image.transform.DOScale(1.1f, 0.1f);
                });
                
                image.transform.DORotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("カーソルがボタンに重なった");
            
            foreach (var image in buttonImage)
            {
                image.transform.DOScale(1.1f, 0.1f);
            }
        }
        
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("カーソルがボタンから離れた");
            
            foreach (var image in buttonImage)
            {
                image.transform.DOScale(1.0f, 0.1f);
            }
        }
    }
}