using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitUI : MonoBehaviour
{
    [SerializeField] Transform selectUnitButtonsParent;

    void Start()
    {
        var buttons = selectUnitButtonsParent.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnClick(index));
        }
    }

    void OnClick(int index)
    {
        Debug.Log($"number:{index}");
    }
}
