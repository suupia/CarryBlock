using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Projects.BattleSystem.Scripts;

namespace Carry.UISystem.UI.CarryScene
{
    public class ResultCanvasNet : NetworkBehaviour
    {
        [SerializeField] Button ReStartButton = null!;
        [SerializeField] Button TitleButton = null!;

        void Start()
        {
            if (!HasStateAuthority) return;
            
            ReStartButton.onClick.AddListener(() =>
            {
                Debug.Log("ReStartButton Clicked");
                SceneTransition.TransitioningScene(Runner, SceneName.CarryScene);
            });
            
            TitleButton.onClick.AddListener(() =>
            {
                Debug.Log("QuitButton Clicked");
                SceneTransition.TransitioningScene(Runner, SceneName.TitleScene);

            });
        }
    }
}