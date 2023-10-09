using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Carry.UISystem.UI;

public class EditMapToolCanvas : MonoBehaviour
{
    [SerializeField] Transform buttonParent = null!;
    [SerializeField] CustomButton buttonPrefab;

    void Start()
    {
        // CustomButtonをインスタンス化する
        for (int i = 0; i < 5; i++)
        {
            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.transform.SetParent(buttonParent.transform);
            
            // ボタンのテキストを設定する
            customButton.SetText($"Stage {i + 1}");
        }
    }
    
    void Update()
    {
        
    }
}
