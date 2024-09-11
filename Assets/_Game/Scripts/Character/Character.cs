using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public  class Character : GameUnit
{
    [SerializeField] private SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform brickPosition;

    private string currentAnimName;
    protected int currentStage = 0;
    protected bool isEndGame = false;
    protected bool isRunning = false;
    protected bool isOnBridge = false;
    protected float interactRange;
    protected float heightOfBrick = 0.5f;

    protected NavMeshAgent agent;
    protected List<Brick> brickList = new();
    public int score { protected set; get; } = 0;
    public bool isCanMoveForward { private set; get; } = true;
    public ColorEnum characterColorEnum { protected set; get; }
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        interactRange = 0.2f;
        score = 0;
        currentStage = 0;
        agent = GetComponent<NavMeshAgent>();
    }

    public bool IsCanMoveForward(Stair stair = null)
    {
        if (stair == null)
            return true;
        if (stair.stairColor == characterColorEnum)
            return true;
        if (isOnBridge && brickList.Count > 0)
            return true;
        if (stair.stairColor != characterColorEnum && brickList.Count == 0)
            return false;
        return true;
    }
    private void ReachEndPoint()
    {
        if (isEndGame) return;
        Debug.Log("Finish");
        GameManager.Ins.winner = this;
        GameManager.Ins.ChangeState(GameState.Finish);
    }

    private void CheckStair(Stair stair)
    {
        if (stair.stairColor != characterColorEnum && brickList.Count > 0)
        {
            isCanMoveForward = true;
            stair.OnChangeColor(characterColorEnum);
            RemoveBrick();
        }
    }

    private void CheckBrick(Brick brick)
    {
        if (characterColorEnum != brick.BrickColorEnum || !brick.isShow())
            return;
        brick.OnCollectBox();
        AddBrick();
    }

    protected virtual void AddBrick()
    {
        if (isEndGame) return;
        Brick brickClone = Instantiate(brickPrefab, transform);
        brickClone.transform.localPosition = Vector3.zero;
        brickClone.OnChangeColor(characterColorEnum);
        brickClone.OnHideCollision(true);
        brickList.Add(brickClone);
        HandlerBrickHeight();
        score++;
    }

    protected virtual void RemoveBrick()
    {
        if (brickList.Count > 0)
        {
            brickList[brickList.Count - 1].OnRemoveBox();
            brickList.RemoveAt(brickList.Count - 1);
        }
    }

    protected virtual void RemoveAllBrick()
    {
        if (brickList.Count <= 0) return;
        for (int i = brickList.Count - 1; i > -1; i--)
        {
            brickList[i].OnRemoveBox();
            brickList.RemoveAt(i);
        }
    }
    private void HandlerBrickHeight()
    {
        if (brickList.Count == 0) return;
        Vector3 newHeightY = brickPosition.localPosition;
        newHeightY.y += Mathf.Abs((brickList.Count - 1) * heightOfBrick);
        brickList[brickList.Count - 1].transform.localPosition = new Vector3(newHeightY.x, newHeightY.y, newHeightY.z);
    }

    public void OnChangeColor(Material material,ColorEnum colorEnum)
    {
        colorRenderer.material = material;
        characterColorEnum = colorEnum;
    }

    public virtual void OnStartGame()
    {
        agent.enabled = true;
        isEndGame = false;
    }
    public virtual void OnPrepareGame()
    {
        OnInit();
        ChangeAnim("Idle");
    }

    public virtual void OnEndGame()
    {
        isEndGame = true;
        agent.enabled = false;
        RemoveAllBrick();
    }

    public void OnResult(Transform transform,int rank)
    {
        this.transform.position = transform.position;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        if (rank == 0)
            ChangeAnim(Constants.WIN_ANIM);
        else
            ChangeAnim(Constants.LOSE_ANIM);
    }

    public virtual void OnSetting()
    {
        
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    protected void IsRunningAnim(bool isRunning)
    {
        anim.SetBool(Constants.RUN_ANIM, isRunning);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Constants.BRICK_TAG))
        {
            Brick brick = collider.GetComponent<Brick>();
            CheckBrick(brick);
        }
        else if (collider.CompareTag(Constants.STAGE_TAG))
        {
            Stage stage = collider.GetComponent<Stage>();
            if (currentStage > (int)stage.GetStageEnum())
                return;
            currentStage++;
            LevelManager.Ins.CharacterMoveToNextStage(currentStage, characterColorEnum);
        }
        else if (collider.CompareTag(Constants.STAIR_TAG))
        {
            Stair stair = collider.GetComponent<Stair>();
            isCanMoveForward = IsCanMoveForward(stair);
            if (isCanMoveForward)
                Debug.Log("true");
            isOnBridge = true;
            CheckStair(stair);
        }
        else if (collider.CompareTag(Constants.WIN_TAG))
        {
            ReachEndPoint();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isOnBridge = false;
        isCanMoveForward = true;
    }

    public void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
    }

}
