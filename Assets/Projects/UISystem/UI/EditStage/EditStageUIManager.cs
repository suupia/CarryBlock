using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
        stageButtons.ForEach(stage => stage.onClick.AddListener(() => detailPanel.SetActive(true)));
        
        mapButtons.ForEach(map => map.onClick.AddListener(() => SceneManager.LoadScene("EditMapForPlayerScene")));
    }
}
