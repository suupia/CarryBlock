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
public class TitleUIManager : MonoBehaviour
{
    [Header("Title")] [SerializeField] CustomButton soloPlayButton;

    [SerializeField] CustomButton multiPlayButton;
    [SerializeField] CustomButton mapMakerButton;
    [SerializeField] CustomButton optionButton;
    [SerializeField] CustomButton closeOptionButton;
    [SerializeField] CustomButton musicRoomButton;
    [SerializeField] CustomButton quitButton;


    [Header("JoinPanel")] [SerializeField] GameObject joinPanel;
    [SerializeField] CustomButton joinAsHostButton;
    [SerializeField] CustomButton joinAsClientButton;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] CustomButton backButton;

    [SerializeField] GameObject optionCanvas;

    string RoomName => roomNameInputField.text;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(joinAsHostButton);
        Assert.IsNotNull(joinAsClientButton);
        Assert.IsNotNull(roomNameInputField);

        var titleInitializer = FindObjectOfType<TitleInitializer>();
        Assert.IsNotNull(titleInitializer, "TitleInitializerをシーンに配置してください");


        joinAsClientButton.AddListener(() => titleInitializer.StartGame(RoomName, GameMode.Client));
        //GameMode.Hostとして扱うかは未定。仮でAutoHostOrClientに設定
        //もし、GameMode.Hostかつ同じルーム名で始めた場合はStartGameExceptionがthrowされる
        joinAsHostButton.AddListener(() => titleInitializer.StartGame(RoomName, GameMode.AutoHostOrClient));


        joinPanel.SetActive(false);


        soloPlayButton.Interactable = false;

        multiPlayButton.AddListener(() => joinPanel.SetActive(true));

        mapMakerButton.AddListener(() => SceneManager.LoadScene("LocalEditStageScene"));

        optionButton.Interactable = true;
        optionButton.AddListener( () => optionCanvas.SetActive(true));
        closeOptionButton.AddListener( () => optionCanvas.SetActive(false));
        optionCanvas.SetActive(false); // 最初は非表示 （最後に処理する）
        
        musicRoomButton.Interactable = false;

        quitButton.AddListener(Quit);

        backButton.AddListener(() => joinPanel.SetActive(false));
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