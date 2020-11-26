using System;
using MightyAttributes;
using PathCreation;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PathCreator _track;
    [SerializeField] private Transform _pathsTransform;
    [SerializeField] private int _pathsCount;
    [SerializeField] private float _pathWidth = .2f;
    [SerializeField] private float _pathYOffset = .5f;

    [SerializeField, ReadOnly] private PathCreator[] _paths;

    public PathCreator GetPath(int index) => index < _paths.Length ? _paths[index] : null;

#if UNITY_EDITOR
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
        var trackPointCount = trackPath.NumPoints;

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
                var time = (float) j / (trackPointCount - 1);

                var normal = trackPath.GetNormal(time);
                var direction = trackPath.GetDirection(time);

                var positionOffset = xOffset * (normal - direction) + _pathYOffset * trackPath.up;

                var point = trackBezierPath.GetPoint(j);

                _paths[i].bezierPath.SetPoint(j, point + positionOffset);
            }
        }
    }
#endif
}