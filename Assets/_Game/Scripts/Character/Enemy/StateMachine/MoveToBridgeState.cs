using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBridgeState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {

    }


    public void OnExecute(Enemy enemy)
    {
        if (enemy.BrickList.Count > 0)
        {
            enemy.Move(LevelManager.Ins.endPointTransform);
        }
        else
        {
            enemy.ChangeState(new CollectBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
