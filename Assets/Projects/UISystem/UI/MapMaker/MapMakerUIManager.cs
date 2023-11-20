#nullable enable
using DG.Tweening;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using VContainer;

//仮クラス。長谷川くんのスクリプトと統合するかも
public class MapMakerUIManager : MonoBehaviour
{
    [SerializeField] GameObject playingCanvas;
    [SerializeField] GameObject editingCanvas;
    [SerializeField] Transform editingCameraTransform;
    [SerializeField] Transform testPlayingCameraTransform;

    readonly float _cameraMoveDuration = 0.5f;
    readonly Ease _easing = Ease.InOutExpo;

    MapTestPlayStarter _mapTestPlayStarter = null!;
    Camera _camera;
    
    [Inject]
    public void Construct(MapTestPlayStarter mapTestPlayStarter)
    {
        _mapTestPlayStarter = mapTestPlayStarter;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        SwitchToEditing();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var canPlay = _mapTestPlayStarter.Start(SwitchToEditing);
            if (canPlay)
            {
                SwitchToTestPlaying();
            }
        }
    }

    void SwitchToEditing()
    {
        editingCanvas.SetActive(true);
        playingCanvas.SetActive(false);
        SwitchCameraToEditing();
    }

    void SwitchToTestPlaying()
    {
        editingCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        SwitchCameraToTestPlaying();
    }

    void SwitchCameraToEditing()
    {
        MoveCamera(editingCameraTransform);
    }

    void SwitchCameraToTestPlaying()
    {
        MoveCamera(testPlayingCameraTransform);
    }

    void MoveCamera(Transform targetTransform)
    {
        _camera.transform.DOMove(targetTransform.position, _cameraMoveDuration).SetEase(_easing);
        _camera.transform.DORotate(targetTransform.rotation.eulerAngles, _cameraMoveDuration)
            .SetEase(_easing);
    }
}