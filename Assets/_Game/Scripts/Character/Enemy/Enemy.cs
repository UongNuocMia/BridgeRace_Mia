using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    private const string IS_RUNNING = "IsRunning";

    [SerializeField] private Animator anim;

    public int randomBrick;
    private Brick nearstBrick;
    private IState<Enemy> currentState;

    public Brick NearstBrick => nearstBrick;
    public List<Brick> BrickList => brickList;

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        randomBrick = Random.Range(5, 10);
    }


    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        RaycastScan();
        Debug.DrawRay(transform.forward, transform.forward * 5, Color.green);
    }
    public void Move(Transform target)
    {
        agent.isStopped = !isCanMoveForward;
        Vector3 moveDirection = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 destination = moveDirection; 
        agent.speed = speed;
        agent.SetDestination(destination);
        anim.SetBool(IS_RUNNING, true);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, 0);
    }

    protected override void AddBrick()
    {
        base.AddBrick();
        FindNearestBrick();
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
}
