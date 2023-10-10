using System.Collections;
using System.Collections.Generic;
using Carry.GameSystem.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobbySystem : NetworkBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // SceneManager.LoadScene("LobbyScene");
            SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }
    }
#endif
}
