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
        [SerializeField] Image frameImage = null!;
        [SerializeField] Image mainImage = null!;
        [SerializeField] TMP_Text textMeshPro = null!;

        AudioSource? _audioSource;
        
        readonly float _clickInterval = 0.1f;
        
        bool _isClickable = true;
        bool _isHovering;
        float _clickTime;

        public bool Interactable
        {
            get => frameImage.raycastTarget;
            set
            {
                frameImage.raycastTarget = value;

                var alpha = value ? 1.0f : 0.5f; //直接DoFadeの引数に入れるとコンパイルが通らん，なぜ？
                frameImage.DOFade(alpha, 0.1f);
            }
        }

        public void SetImage(Texture2D tex)
        {
            mainImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        
        public void SetText(string text)
        {
            textMeshPro.text = text;
        }
        
        void Start()
        {
            Assert.IsNotNull(clickSound);
            Assert.IsNotNull(frameImage);
            Assert.IsNotNull(mainImage);
            Assert.IsNotNull(textMeshPro);

            _audioSource = FindObjectOfType<AudioSource>();
            Interactable = true;
            
            textMeshPro.alpha = 0.0f;
        }

        void Update()
        {
            if (!_isClickable && _clickTime + _clickInterval < Time.time)
            {
                _isClickable = true;
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) { return; }

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

            ClickAction.Invoke();

            Debug.Log("ボタンがクリックされた(押され，ドラッグされずに離された)");

            frameImage.transform.DOScale(1.2f, 0.1f).OnComplete(() => { frameImage.transform.DOScale(_isHovering ? 1.1f : 1.0f, 0.1f); });

            mainImage.transform.DOScale(1.3f, 0.1f).OnComplete(() => { mainImage.transform.DOScale(_isHovering ? 1.1f : 1.0f, 0.1f); });
            mainImage.transform.DORotate(new Vector3(0, 0, 360), 0.1f, RotateMode.FastBeyond360);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) { return; }
            
            _isHovering = true;
            
            Debug.Log("カーソルがボタンに重なった");

            frameImage.transform.DOScale(1.1f, 0.1f);
            mainImage.transform.DOScale(1.1f, 0.1f);
            textMeshPro.alpha = 1.0f;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("カーソルがボタンから離れた");
            
            _isHovering = false;

            frameImage.transform.DOScale(1.0f, 0.1f);
            mainImage.transform.DOScale(1.0f, 0.1f);
            textMeshPro.alpha = 0.0f;
        }
    }
}