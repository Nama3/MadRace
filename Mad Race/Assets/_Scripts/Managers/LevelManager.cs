using MightyAttributes;
using PathCreation;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private PathCreator _track;
    [SerializeField] private Transform _pathsTransform;
    [SerializeField] private int _pathsCount;
    [SerializeField] private float _pathWidth = .2f;
    [SerializeField] private float _pathStartOffset, _pathEndOffset;

    [SerializeField, ReadOnly] private PathCreator[] _paths;

    private void Start() => Init();

    public void Init()
    {
        Instance = this;

        for (var i = 0; i < _paths.Length; i++)
            RunnersManager.Instance.SetRunnerPath(i, _paths[i]);

        RunnersManager.Instance.InitRunners();
    }

    public void FinishLevel(BaseRunnerBehaviour runner)
    {
        RunnersManager.Instance.StopRunners();
        if (runner is PlayerBehaviour)
            GameManager.Instance.LoadNextLevel();
        else
            GameManager.Instance.ReloadLevel();
    }

#if UNITY_EDITOR

    #region Editor

    [Button]
    private void GeneratePaths()
    {
        if (_pathsCount == 0 || !_pathsTransform) return;
        if (!ComponentUtility.CopyComponent(_track)) return;

        _paths = new PathCreator[_pathsCount];

        while (_pathsTransform.childCount > 0)
            DestroyImmediate(_pathsTransform.GetChild(0).gameObject);

        var trackTransform = _track.transform;
        var trackPath = _track.path;
        var trackBezierPath = _track.bezierPath;

        for (var i = 0; i < _pathsCount; i++)
        {
            var go = new GameObject($"Path {i}");
            go.transform.SetParent(_pathsTransform);

            var xOffset = Mathf.Lerp(-_pathWidth, _pathWidth, (float) i / (_pathsCount - 1));

            go.transform.position = trackTransform.position;
            _paths[i] = go.AddComponent<PathCreator>();
            ComponentUtility.PasteComponentValues(_paths[i]);

            for (var j = 0; j < trackBezierPath.NumPoints; j++)
            {
                var point = trackBezierPath.GetPoint(j);
                var time = (float) j / (trackBezierPath.NumPoints - 1);

                var normal = trackPath.GetNormal(time, EndOfPathInstruction.Stop);
                var direction = trackPath.GetDirection(time, EndOfPathInstruction.Stop);

                var positionOffset = xOffset * -Vector3.Cross(normal, direction);

                if (j < 2) positionOffset += direction * _pathStartOffset;
                else if (j > trackBezierPath.NumPoints - 3) positionOffset += -direction * _pathEndOffset;

                _paths[i].bezierPath.SetPoint(j, point + positionOffset, true);
            }
        }
    }

    #endregion /Editor

#endif
}