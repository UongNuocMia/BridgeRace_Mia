public class IdleState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Constants.IDLE_ANIM);
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
