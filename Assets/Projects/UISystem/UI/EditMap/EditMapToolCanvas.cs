using UnityEngine;
using Carry.UISystem.UI;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;

public class EditMapToolCanvas : MonoBehaviour
{
    [SerializeField] Transform buttonParent = null!;
    [SerializeField] CustomButton buttonPrefab;

    void Start()
    {
        var editMapForPlayerInput = FindObjectOfType<EditMapForPlayerInput>();


        var customButton = Instantiate(buttonPrefab, buttonParent);
        customButton.Init();
        customButton.SetText("Erase Mode");
        customButton.AddListener(() => editMapForPlayerInput.ToggleEraseMode());
        
        customButton = Instantiate(buttonPrefab, buttonParent);
        customButton.Init();
        customButton.SetText("Reset Map");
        customButton.AddListener(() => editMapForPlayerInput.ClearMap());
        
        customButton = Instantiate(buttonPrefab, buttonParent);
        customButton.Init();
        customButton.SetText("Redo");
        customButton.AddListener(() => editMapForPlayerInput.ClearMap());
        
        customButton = Instantiate(buttonPrefab, buttonParent);
        customButton.Init();
        customButton.SetText("Undo");
        customButton.AddListener(() => editMapForPlayerInput.ClearMap());
    }
    
    void Update()
    {
        
    }
}
