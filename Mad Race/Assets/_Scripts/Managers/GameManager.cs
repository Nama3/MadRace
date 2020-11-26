using MightyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, FindAssets, Reorderable(false, options:ArrayOption.DisableSizeField)] private LevelModel[] _levels;

    [SerializeField, FindObject, ReadOnly] private RunnersManager _runnersManager;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _runnersManager.Init();
        LoadLevel(SavedDataServices.LevelIndex);
    }

    private void Update()
    {
        _runnersManager.UpdateManager();
    }

    private void FixedUpdate()
    {
        _runnersManager.FixedUpdateManager();
    }
    
    public void LoadNextLevel()
    {
        var index = SavedDataServices.LevelIndex;

        if (IsLevelLoaded(index)) UnloadLevel(index);
        LoadLevel(++index);
    }

    public void LoadLevel(byte index, bool saveProgression = true)
    {
        if (index >= _levels.Length) index = 0;

        if (IsLevelLoaded(index)) UnloadLevel(index);
        if (saveProgression) SavedDataServices.LevelIndex = index;

        SceneManager.LoadScene(_levels[index].SceneIndex, LoadSceneMode.Additive);
    }

    public void UnloadLevel(byte index) => SceneManager.UnloadSceneAsync(_levels[index].SceneIndex);

    public bool IsLevelLoaded(byte index) => _levels[index].IsLoaded();

    public void RestartLevel() => LoadLevel(SavedDataServices.LevelIndex, false);
}