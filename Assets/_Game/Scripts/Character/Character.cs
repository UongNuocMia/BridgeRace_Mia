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
    protected bool isEndGame;
    protected bool isRunning;
    protected bool isOnBridge;
    protected float interactRange;
    protected float heightOfBrick = 0.5f;

    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai
    protected List<Brick> brickList = new();
    public int Score { protected set; get; } = 0;
    public bool IsCanMoveForward { private set; get; } = true;
    public ColorEnum CharacterColorEnum { protected set; get; }
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        interactRange = 0.2f;
        Score = 0;
        currentStage = 0;
        agent = GetComponent<NavMeshAgent>();
    }

    public bool IsCanMoveForwardCheck(Stair stair = null)
    {
        if (stair == null)
            return true;
        if (stair.StairColor == CharacterColorEnum)
            return true;
        if (isOnBridge && brickList.Count > 0)
            return true;
        if (stair.StairColor != CharacterColorEnum && brickList.Count == 0)
            return false;
        return true;
    }
    private void ReachEndPoint()
    {
        if (isEndGame) return;
        GameManager.Ins.Winner = this;
        GameManager.Ins.ChangeState(GameState.Finish);
    }

    private void CheckStair(Stair stair)
    {
        if (stair.StairColor != CharacterColorEnum && brickList.Count > 0)
        {
            IsCanMoveForward = true;
            stair.OnChangeColor(CharacterColorEnum);
            RemoveBrick();
        }
    }

    private void CheckBrick(Brick brick)
    {
        if (CharacterColorEnum != brick.BrickColorEnum || !brick.isShow())
            return;
        brick.OnCollectBox();
        AddBrick();
    }

    protected virtual void AddBrick()
    {
        if (isEndGame) return;
        Brick brickClone = Instantiate(brickPrefab, brickPosition);
        brickClone.transform.localPosition = Vector3.zero;
        brickClone.OnChangeColor(CharacterColorEnum);
        brickClone.OnHideCollision(true);
        brickList.Add(brickClone);
        HandlerBrickHeight();
        Score++;
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
        Vector3 newHeightY = Vector3.zero;
        newHeightY.y += Mathf.Abs((brickList.Count - 1) * heightOfBrick);
        brickList[brickList.Count - 1].transform.localPosition = newHeightY;
    }

    public void OnChangeColor(Material material,ColorEnum colorEnum)
    {
        colorRenderer.material = material;
        CharacterColorEnum = colorEnum;
    }

    public virtual void OnStartGame()
    {
        agent.enabled = true;
        isEndGame = false;
    }
    public virtual void OnPrepareGame()
    {
        OnInit();
        ChangeAnim(Constants.IDLE_ANIM);
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

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Constants.BRICK_TAG))
        {
            Brick brick = collider.GetComponent<Brick>();
            CheckBrick(brick);
        }
        if (collider.CompareTag(Constants.STAGE_TAG))
        {
            Stage stage = collider.GetComponent<Stage>();
            if (currentStage > (int)stage.GetStageEnum())
                return;
            currentStage++;
            LevelManager.Ins.CharacterMoveToNextStage(currentStage, CharacterColorEnum);
        }
        if (collider.CompareTag(Constants.STAIR_TAG))
        {
            Stair stair = collider.GetComponent<Stair>();
            IsCanMoveForward = IsCanMoveForwardCheck(stair);
            isOnBridge = true;
            CheckStair(stair);
        }
        if (collider.CompareTag(Constants.WIN_TAG))
        {
            ReachEndPoint();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isOnBridge = false;
        IsCanMoveForward = true;
    }
}
