using System;
using System.Collections;
using System.Collections.Generic;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using TMPro;
using UnityEngine;

public class EditMapPrepareInitializer : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    async void Start()
    {
        // ローディングアニメーションを開始する
        StartCoroutine(LoadingAnimation());
        
        // シーン開始直後にEditMapSceneに遷移する
        var runnerManager = FindObjectOfType<NetworkRunnerManager>();
        await runnerManager.AttemptStartScene();
        Debug.Log("Transitioning to EditMapScene");
        SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.EditMapScene);
    }
    
     IEnumerator LoadingAnimation()
    {
        while (true)
        {
            loadingText.text = "Load.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Load..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Load...";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
