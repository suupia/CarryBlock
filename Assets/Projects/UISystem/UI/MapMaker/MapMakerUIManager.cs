using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using VContainer;
#nullable enable
//仮クラス。長谷川くんのスクリプトと統合するかも
public class MapMakerUIManager : MonoBehaviour
{
    [SerializeField] GameObject playingCanvas;
    [SerializeField] GameObject editingCamera;
    [SerializeField] GameObject testPlayingCamera;


    MapTestPlayStarter _mapTestPlayStarter = null!;


    [Inject]
    public void Construct(
        MapTestPlayStarter mapTestPlayStarter)
    {
        _mapTestPlayStarter = mapTestPlayStarter;
    }

    // Start is called before the first frame update
    void Start()
    {
        playingCanvas.SetActive(false);
        SwitchCameraToEditing();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var canPlay = _mapTestPlayStarter.Start(() =>
            {
                playingCanvas.SetActive(false);
                SwitchCameraToEditing();
            });
            if (canPlay)
            {
                playingCanvas.SetActive(true);
                SwitchCameraToTestPlaying();
            }
        }
    }

    void SwitchCameraToEditing()
    {
        testPlayingCamera.SetActive(false);
        editingCamera.SetActive(true);
    }

    void SwitchCameraToTestPlaying()
    {
        testPlayingCamera.SetActive(true);
        editingCamera.SetActive(false);
    }
}