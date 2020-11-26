using MightyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Model/Level Model", fileName = "LevelModel")]
public class LevelModel : BaseModel
{
    [SerializeField, SceneDropdown] private byte _sceneIndex;

    public byte SceneIndex => _sceneIndex;

    public bool IsLoaded() => SceneManager.GetSceneByBuildIndex(SceneIndex).isLoaded;
}