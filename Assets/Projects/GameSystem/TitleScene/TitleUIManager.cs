using Fusion;
using Projects.BattleSystem.TitleScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

/// <summary>
///     タイトルのUI要素を管理する
///     ボタンのコールバックの設定などを行う
/// </summary>
public class TitleUIManager : MonoBehaviour
{
    [SerializeField] Button joinAsHostButton;

    [SerializeField] Button joinAsClientButton;

    [SerializeField] TMP_InputField roomNameInputField;

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
    }
}