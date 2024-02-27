using Carry.GameSystem.Scripts;
using Carry.GameSystem.TitleScene.Scripts;
using Carry.UISystem.UI;
using Fusion;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assert = UnityEngine.Assertions.Assert;

/// <summary>
///     タイトルのUI要素を管理する
///     ボタンのコールバックの設定などを行う
/// </summary>
public class TitleUIManagerLocal : MonoBehaviour
{
    [Header("Title")] 
    [SerializeField] CustomButton playButton;
    [SerializeField] CustomButton mapMakerButton;
    [SerializeField] CustomButton optionButton;
    [SerializeField] CustomButton closeOptionButton;
    [SerializeField] CustomButton musicRoomButton;
    [SerializeField] CustomButton quitButton;
    
    [SerializeField] GameObject optionCanvas;
    

    // Start is called before the first frame update
    void Start()
    {
        var titleInitializer = FindObjectOfType<TitleInitializer>();

        playButton.AddListener(() => SceneTransition.TransitioningScene(SceneName.LobbySceneLocal));

        mapMakerButton.AddListener(() => SceneManager.LoadScene("LocalEditStageScene"));

        optionButton.Interactable = true;
        optionButton.AddListener( () => optionCanvas.SetActive(true));
        closeOptionButton.AddListener( () => optionCanvas.SetActive(false));
        optionCanvas.SetActive(false); // 最初は非表示 （最後に処理する）
        
        musicRoomButton.Interactable = false;

        quitButton.AddListener(Quit);

    }

    void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }
}