using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EditStageUIManager : MonoBehaviour
{
    [SerializeField] List<Button> stageButtons;
    [SerializeField] List<Button> mapButtons;

    [SerializeField] GameObject detailPanel;

    [SerializeField] Button detailPanelBackButton;
    // Start is called before the first frame update
    void Start()
    {

        detailPanel.SetActive(false);
        detailPanelBackButton.onClick.AddListener(() => detailPanel.SetActive(false));
        
        SetUpStageButtons();
        
    }

    void SetUpStageButtons()
    {
        var stages = StageIOSystem.GetStages();

        Assert.AreEqual(
            stages.Count, stageButtons.Count, 
            "UIに表示できるステージの数と実際に保存されているステージの数が一致しません"
        );
        
        for (var i = 0; i < stageButtons.Count; i++)
        {
            var stageButton = stageButtons[i];
            var stage = stages[i];
            var stageButtonText = stageButton.GetComponentInChildren<TMP_Text>();

            Assert.IsNotNull(stageButtonText);
            
            stageButtonText.text = stage.name;
            stageButton.onClick.AddListener(() =>
            {
                //ステージをタッチするごとにマップを読み込み反映させる
                //detailPanelを再利用するイメージ
                for (var j = 0; j < mapButtons.Count; j++)
                {
                    var mapButton = mapButtons[j];
                    var mapInfo = stage.mapInfos[j];
                    var mapButtonText = mapButton.GetComponentInChildren<TMP_Text>();
                    
                    Assert.IsNotNull(mapButtonText);

                    mapButtonText.text = mapInfo.name;
                    mapButton.onClick.AddListener(() => SceneManager.LoadScene("EditMapForPlayerScene"));
                }

                detailPanel.SetActive(true);
            });
        }
    }
}
