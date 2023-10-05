using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobbySystem : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }
#endif
}
