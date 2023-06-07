using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.GameSystem.Scripts;
using UnityEngine;

namespace Nuts.GameSystem.TitleScene.Scripts
{
    public class TitleInitializer : MonoBehaviour
    {
        //Get roomName from UI component.
        public string RoomName { get; set; }


        //Called by UI component
        public async void StartGameWithRoomName()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene("RoomName");
            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.LobbyScene);
        }
    }
}