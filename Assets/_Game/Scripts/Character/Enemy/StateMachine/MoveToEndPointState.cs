
using UnityEngine;

public class MoveToEndPointState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Move(LevelManager.Ins.EndPointTransform);
    }

    public void OnExecute(Enemy enemy)
    {
        if (!enemy.IsCanMoveForward)
        {
            enemy.ChangeState(new CollectBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
