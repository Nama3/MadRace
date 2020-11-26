using MightyAttributes;
using PathCreation;
using UnityEngine;

public class RunnersManager : MonoBehaviour
{
    public static RunnersManager Instance { get; private set; }

    [SerializeField, FindObjects, Reorderable(false, options: ArrayOption.DisableSizeField)]
    private BaseRunnerBehaviour[] _runners;

    public void Init() => Instance = this;

    public void InitRunners()
    {
        foreach (var runner in _runners) runner.Init();
    }

    public void UpdateManager()
    {
        foreach (var runner in _runners)
            runner.UpdateBehaviour();
    }

    public void FixedUpdateManager()
    {
        foreach (var runner in _runners)
            runner.FixedUpdateBehaviour();
    }

    public void SetRunnerPath(int runnerIndex, PathCreator path)
    {
        if (runnerIndex < _runners.Length)
            _runners[runnerIndex].SetPath(path);
    }
}
