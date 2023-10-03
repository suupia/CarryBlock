using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.GameSystem.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

public class EditMapPrepareInitializer : MonoBehaviour
{
    [Tooltip("ファイル名に使用するマップキーを設定してください")]
    [SerializeField] MapKey mapKey;
    [SerializeField] TextMeshProUGUI mapKeyText;
    [SerializeField] TextMeshProUGUI loadingText;
           

    async void Start()
    {
        // ローディングアニメーションを開始する
        StartCoroutine(LoadingAnimation());
        
        // マップキーの確認用のテキストを表示する
        mapKeyText.text =  $"MapKey : <color=\"red\">{mapKey.ToString()}</color>";
        
        // マップキーをコンテナに登録して、次のシーンに渡す
        var mapKeyContainer = new GameObject("MapKeyContainer");
        mapKeyContainer.AddComponent<MapKeyContainer>().SetMapKey(mapKey);
        
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
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
