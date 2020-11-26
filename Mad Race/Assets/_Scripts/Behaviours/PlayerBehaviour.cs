using UnityEngine;

public class PlayerBehaviour : BaseRunnerBehaviour
{
    protected override void UpdateRunner()
    {
        CanRun = Input.GetMouseButton(0);
    }

    protected override void FixedUpdateRunner()
    {
        
    }
}