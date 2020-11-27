public class BotBehaviour : BaseRunnerBehaviour
{
    protected override void OnInit()
    {
    }

    protected override void OnPlay() => CanRun = true;
    
    protected override void OnUpdate()
    {
    }

    protected override void OnFixedUpdate()
    {
    }
    
    protected override void OnFallOutOfMap() => ResetRigidbody();
}
