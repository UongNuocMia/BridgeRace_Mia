
public class MoveToBridgeState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Move(LevelManager.Ins.endPointTransform);
    }


    public void OnExecute(Enemy enemy)
    {

        if (enemy.BrickList.Count <= 0) // CHANGE LATER, VÔ LÝ Ở CHỖ KHI CẦU FULL GẠCH MÀU ĐÓ NHƯNG BRICKLIST = 0 THÌ QUAY LẠI
        {
            enemy.ChangeState(new CollectBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
