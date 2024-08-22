using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectBrickState : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.FindNearestBrick();
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.randomBrick == enemy.BrickList.Count)
        {
            enemy.ChangeState(new IdleState());
        }
        if (enemy.NearstBrick != null)
        {
            enemy.Move(enemy.NearstBrick.transform);
        }
    }
    
    public void OnExit(Enemy t)
    {

    }

}
