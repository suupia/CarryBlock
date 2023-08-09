using Fusion;
using Projects.BattleSystem.TitleScene.Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

/// <summary>
///     タイトルのUI要素を管理する
///     ボタンのコールバックの設定などを行う
/// </summary>
public class TitleUIManager : MonoBehaviour
{
    [Header("Title")] [SerializeField] Button soloPlayButton;

    [SerializeField] Button multiPlayButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button musicRoomButton;
    [SerializeField] Button quitButton;


    [Header("JoinPanel")] [SerializeField] GameObject joinPanel;
    [SerializeField] Button joinAsHostButton;
    [SerializeField] Button joinAsClientButton;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] Button backButton;

    string RoomName => roomNameInputField.text;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(joinAsHostButton);
        Assert.IsNotNull(joinAsClientButton);
        Assert.IsNotNull(roomNameInputField);

        var titleInitializer = FindObjectOfType<TitleInitializer>();
        Assert.IsNotNull(titleInitializer, "TitleInitializerをシーンに配置してください");


        joinAsClientButton.onClick.AddListener(() => titleInitializer.StartGame(RoomName, GameMode.Client));
        //GameMode.Hostとして扱うかは未定。仮でAutoHostOrClientに設定
        //もし、GameMode.Hostかつ同じルーム名で始めた場合はStartGameExceptionがthrowされる
        joinAsHostButton.onClick.AddListener(() => titleInitializer.StartGame(RoomName, GameMode.AutoHostOrClient));


        soloPlayButton.interactable = false;

        multiPlayButton.onClick.AddListener(() => joinPanel.SetActive(true));

        optionButton.interactable = false;

        musicRoomButton.interactable = false;

        quitButton.onClick.AddListener(Quit);

        backButton.onClick.AddListener(() => joinPanel.SetActive(false));
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