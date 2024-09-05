using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectBrickState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        if (enemy.BrickList.Count <= enemy.randomBrick)
        {
            enemy.FindNearestBrick();
            if (enemy.nearstBrick != null)
            {
                enemy.Move(enemy.nearstBrick.transform);
            }
        }
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.BrickList.Count >= enemy.randomBrick || enemy.nearstBrick == null)
        {
            enemy.ChangeState(new MoveToBridgeState());
            //enemy.Move(LevelManager.Ins.endPointTransform);
        }
    }
    
    public void OnExit(Enemy t)
    {

    }

}
