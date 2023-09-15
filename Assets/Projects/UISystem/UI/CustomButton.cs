
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Projects.UISystem.UI
{
    [RequireComponent(typeof(Button))]
    public class CustomButton : UIBehaviour
    {
        
        [SerializeField] AudioClip clickSound;
        Button _button;
        AudioSource _audioSource;
        
        protected override void Awake()
        {
            _button = GetComponent<Button>();
            _audioSource = FindObjectOfType<AudioSource>();
            
            // audioSourceを取得せずに、 Playのときに　 SoundManager.Instance.Play(clickSound);　という処理にしてもよいかも
        }
        
        public void AddListenerWithSound(UnityAction action)
        {
            _button.onClick.AddListener(() =>
            {
                action();
               _audioSource.PlayOneShot(clickSound);
            });
        }
    }
}