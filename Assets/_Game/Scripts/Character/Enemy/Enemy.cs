using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{ 
    private IState<Enemy> currentState;
    public int randomBrick { private set; get; }
    public Brick nearstBrick { private set; get; }
    public List<Brick> BrickList => brickList;

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        isRunning = false;
        randomBrick = Random.Range(5, 10);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
            currentState.OnExecute(this);
        if (GameManager.IsState(GameState.GamePlay))
        {
            if (isRunning)
                ChangeAnim(Constants.RUN_ANIM);
            else
                ChangeAnim(Constants.IDLE_ANIM);
        }
    }
    public void Move(Transform target)
    {
        isRunning = true;
        Vector3 moveDirection = new Vector3(target.position.x, target.position.y, target.position.z);
        Vector3 destination = moveDirection; 
        agent.speed = speed;
        agent.SetDestination(destination);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, 0);
    }

    protected override void AddBrick()
    {
        base.AddBrick();
        if (brickList.Count <= randomBrick && GameManager.IsState(GameState.GamePlay))
            ChangeState(new CollectBrickState());
    }
    public void ChangeState(IState<Enemy> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void FindNearestBrick()
    {
        nearstBrick = null;
        List<Brick> brickLis = Spawner.Ins.brickDict.Where(brick => brick.Value == characterColorEnum).Select(brick => brick.Key).ToList();
        if (brickLis.Count > 0)
        {
            float distance = 100f;
           
            foreach (Brick brick in brickLis)
            {
                if (brick.isShow() && distance > Vector3.Distance(transform.position, brick.transform.position) )
                {
                    nearstBrick = brick;
                }
            }
        }
        
    }

    public override void OnStartGame()
    {
        base.OnStartGame();
        ChangeState(new CollectBrickState());
    }
    public override void OnEndGame()
    {
        base.OnEndGame();
        isRunning = false;
        Move(transform);
        ChangeState(new IdleState());
    }

    public override void OnPrepareGame()
    {
        base.OnPrepareGame();
        ChangeState(new IdleState());
    }
    public override void OnSetting()
    {
        Move(transform);
        ChangeState(new IdleState());
    }
}
