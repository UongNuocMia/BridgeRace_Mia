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
    }
    public void Move(Transform target)
    {
        Vector3 moveDirection = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 destination = moveDirection;
        agent.speed = speed;
        agent.SetDestination(destination);
        float rotateSpeed = 5f;
        anim.SetBool(IS_RUNNING, true);
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
        Dictionary< Brick,ColorEnum> brickDictOnGround = LevelManager.Ins.brickDict;
        if(brickDictOnGround.Count > 0)
        {
            float distance = 10;
            foreach (var item in brickDictOnGround)
            {
                if (item.Value == characterColorEnum && distance < Vector3.Distance(this.transform.position, item.Key.transform.position))
                {
                    nearstBrick = item.Key;
                }
            }
        }
        
    }
}
