public class IdleState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.SetRunning(false);
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
