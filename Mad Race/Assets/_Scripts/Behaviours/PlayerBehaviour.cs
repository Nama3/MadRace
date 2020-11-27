using UnityEngine;

public class PlayerBehaviour : BaseRunnerBehaviour
{
    protected override void OnInit() => GUIManager.Instance.ShowPressText(true);

    protected override void OnPlay()
    {
    }

    protected override void OnUpdate()
    {
        switch (State)
        {
            case RunnerState.Init:
                if (Input.GetMouseButton(0))
                {
                    RunnersManager.Instance.PlayRunners();
                    GUIManager.Instance.ShowPressText(false);
                }

                break;
            case RunnerState.Playing:
                CanRun = Input.GetMouseButton(0);
                break;
        }
    }

    protected override void OnFixedUpdate()
    {
        
    }
    protected override void OnFallOutOfMap()
    {
        GameManager.Instance.ReloadLevel();
    }
}