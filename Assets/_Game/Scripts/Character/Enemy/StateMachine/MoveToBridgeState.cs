
using UnityEngine;

public class MoveToBridgeState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Move(LevelManager.Ins.endPointTransform);
    }


    public void OnExecute(Enemy enemy)
    {

        if (enemy.BrickList.Count <0) // change later
        {
            Debug.Log("Khong di tiep duoc");
            enemy.ChangeState(new CollectBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
