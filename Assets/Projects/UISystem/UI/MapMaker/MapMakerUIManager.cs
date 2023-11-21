#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using DG.Tweening;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

//仮クラス。長谷川くんのスクリプトと統合するかも
public class MapMakerUIManager : MonoBehaviour
{
    [SerializeField] GameObject playingCanvas;
    [SerializeField] GameObject editingCanvas;
    [SerializeField] GameObject worldCanvas;
    [SerializeField] GameObject resultCanvas;

    [SerializeField] Button saveButton;
    [SerializeField] Button returnToEditingButton;
    
    [SerializeField] Transform editingCameraTransform;
    [SerializeField] Transform testPlayingCameraTransform;

    [SerializeField] GameObject cursorCanvas;

    [SerializeField] Button backButton;
    [SerializeField] Button testPlayButton;

    readonly float _cameraMoveDuration = 0.5f;
    readonly Ease _easing = Ease.InOutExpo;

    MapTestPlayStarter _mapTestPlayStarter = null!;
    IMapGetter _mapGetter = null!;
    StageMapSaver _stageMapSaver = null!;
    Camera _camera;
    
    [Inject]
    public void Construct(
        MapTestPlayStarter mapTestPlayStarter,
        IMapGetter mapGetter,
        StageMapSaver stageMapSaver)
    {
        _mapTestPlayStarter = mapTestPlayStarter;
        _mapGetter = mapGetter;
        _stageMapSaver = stageMapSaver;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        returnToEditingButton.onClick.AddListener(SwitchToEditing);
        saveButton.onClick.AddListener(() =>
        {
            _stageMapSaver.Save(_mapGetter.GetMap());
            SwitchToEditing();
        });
        SwitchToEditing();

        backButton.onClick.AddListener(() => SceneManager.LoadScene("LocalEditStageScene"));

        testPlayButton.onClick.AddListener(() =>
        {
            var canPlay = _mapTestPlayStarter.StartTest(isClear =>
            {
                if (isClear)
                    resultCanvas.SetActive(true);
                else
                    SwitchToEditing();
            });
            if (canPlay) SwitchToTestPlaying();
        });
    }
    
    void SwitchToEditing()
    {
        resultCanvas.SetActive(false);
        editingCanvas.SetActive(true);
        worldCanvas.SetActive(true);
        playingCanvas.SetActive(false);
        cursorCanvas.SetActive(true);
        SwitchCameraToEditing();
    }

    void SwitchToTestPlaying()
    {
        resultCanvas.SetActive(false);
        editingCanvas.SetActive(false);
        worldCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        cursorCanvas.SetActive(false);
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