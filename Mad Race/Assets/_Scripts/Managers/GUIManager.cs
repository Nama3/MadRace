using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }

    [SerializeField] private GameObject _pressTextObject;
    
    public void Init() => Instance = this;

    public void ShowPressText(bool show) => _pressTextObject.SetActive(show);
}
