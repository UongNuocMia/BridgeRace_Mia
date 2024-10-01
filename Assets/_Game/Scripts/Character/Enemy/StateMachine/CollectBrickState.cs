
public class CollectBrickState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        if (enemy.BrickList.Count <= enemy.RandomBrick)
        {
            enemy.FindNearestBrick();
            if (enemy.NearstBrick != null)
            {
                enemy.Move(enemy.NearstBrick.transform);
            }
        }
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.BrickList.Count >= enemy.RandomBrick || enemy.NearstBrick == null)
        {
            enemy.ChangeState(new MoveToEndPointState());
            //enemy.Move(LevelManager.Ins.endPointTransform);
        }
    }
    
    public void OnExit(Enemy t)
    {

    }

}
