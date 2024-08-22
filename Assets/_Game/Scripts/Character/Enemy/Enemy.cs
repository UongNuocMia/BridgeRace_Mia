using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{

    private GameUnit nearstBrick;
    public GameUnit NearstBrick => nearstBrick;
    private IState<Enemy> currentState;
    public int randomBrick;

    public List<Brick> BrickList => brickList;

    protected override void OnInit()
    {
        base.OnInit();
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new CollectBrickState());
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
    }
    public void Move(Transform target)
    {
        Vector3 moveDirection = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 destination = moveDirection;
        agent.SetDestination(destination);
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    protected override void AddBrick()
    {
        base.AddBrick();
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
        List<GameUnit> brickListOnGround = LevelManager.Ins.ListBrickOnGround;
        if(brickListOnGround.Count > 0)
        {
            float distance = Vector3.Distance(this.transform.position, brickListOnGround[0].transform.position);
            foreach (var brick in brickListOnGround)
            {
                if (distance < Vector3.Distance(this.transform.position, brick.transform.position))
                {
                    nearstBrick = brick;
                }
            }
        }
        
    }
}
