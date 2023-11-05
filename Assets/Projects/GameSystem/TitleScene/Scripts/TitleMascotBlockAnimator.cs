using UnityEngine;
using DG.Tweening;

namespace Carry.TitleScene.Scripts
{
    public class TitleMascotBlockAnimator : MonoBehaviour
    {
        readonly float _period = 1.0f;
        readonly float _amplitude = 10.0f;
        
        void Start()
        {
            float nowY = transform.localPosition.y;
            
            transform
                .DOLocalMoveY(nowY - _amplitude, _period)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }      
}