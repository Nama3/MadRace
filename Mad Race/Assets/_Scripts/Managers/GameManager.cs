using MightyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, FindAssets] private LevelModel[] _levels;
}
