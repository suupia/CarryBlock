
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#nullable enable

namespace Carry.UISystem.UI
{
    [RequireComponent(typeof(Button))]
    public class CustomButton : UIBehaviour
    {
        [SerializeField] AudioClip clickSound = null!;
        [SerializeField] TextMeshProUGUI? textMeshProUGUI;
        Button? _button;
        Component[]? _childrenComponents;
        AudioSource? _audioSource;

        bool _isInitialized = false;

        protected override void Awake()
        {
            Setup();
            _childrenComponents = GetComponentsInChildren<Component>();
        }
        
        public void Init()
        {
            Setup();
        }

        void Setup()
        {
            if (_isInitialized) return;

            Assert.IsNotNull(clickSound);
            
            _button = GetComponent<Button>();
            _audioSource = FindObjectOfType<AudioSource>();
            
            // audioSourceを取得せずに、 Playのときに　 SoundManager.Instance.Play(clickSound);　という処理にしてもよいかも

            _isInitialized = true;
        }

        public bool Interactable
        {
            get => _button?.interactable ?? false;
            set
            {
                if(_button == null) return;
                if (_childrenComponents != null)
                {
                    foreach (var child in _childrenComponents)
                    {
                        if (child is TextMeshProUGUI textMesh)
                        {
                            textMesh.alpha = value ? 1.0f : 0.5f;
                        }
                    }
                }
                _button.interactable = value;
            }
        }

        public void AddListener(UnityAction action)
        {
            Setup();
            if(_button == null) return;
            _button.onClick.AddListener(() =>
            {
                action();
                if(_audioSource != null) _audioSource.PlayOneShot(clickSound);
                else Debug.LogError($"_audioSource is null");
            });
        }
        
        public void SetText(string text)
        {
            if(textMeshProUGUI == null) return;
            textMeshProUGUI.text = text;
        }
    }
}