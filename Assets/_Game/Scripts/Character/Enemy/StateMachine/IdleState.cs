public class IdleState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Move(enemy.transform);
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
