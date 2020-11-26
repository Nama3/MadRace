using MightyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Level Model", fileName = "LevelModel")]
public class LevelModel : BaseModel
{
    [SerializeField, SceneDropdown] private byte _sceneIndex;

    public byte SceneIndex => _sceneIndex;
}