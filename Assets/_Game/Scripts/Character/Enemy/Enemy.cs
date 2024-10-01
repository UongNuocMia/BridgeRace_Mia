using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{ 
    private IState<Enemy> currentState;
    public int RandomBrick { private set; get; }
    public Brick NearstBrick { private set; get; }
    public List<Brick> BrickList => brickList;


    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        RandomBrick = Random.Range(5, 10);   
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
            currentState.OnExecute(this);
    }
    public void Move(Transform target)
    {
        if (!agent.isOnNavMesh) return;
        Vector3 moveDirection = new Vector3(target.position.x, target.position.y, target.position.z);
        Vector3 destination = moveDirection; 
        agent.speed = speed;
        agent.SetDestination(destination);
        isRunning = destination != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, 0);
        ChangeAnim(Constants.RUN_ANIM);
    }

    protected override void AddBrick()
    {
        base.AddBrick();
        if (brickList.Count <= RandomBrick && GameManager.IsState(GameState.GamePlay))
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
        NearstBrick = null;
        List<Brick> brickLis = Spawner.Ins.BrickDict.Where(brick => brick.Value == CharacterColorEnum).Select(brick => brick.Key).ToList();
        if (brickLis.Count > 0)
        {
            float distance = 100f;
           
            foreach (Brick brick in brickLis)
            {
                if (brick.isShow() && distance > Vector3.Distance(transform.position, brick.transform.position) )
                {
                    NearstBrick = brick;
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
