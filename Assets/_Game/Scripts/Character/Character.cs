using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public  class Character : GameUnit
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected Transform brickPosition;
    [SerializeField] private SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected float speed = 5;

    private string currentAnimName;
    protected int currentStage = 0;
    protected bool isEndGame = false;
    protected bool isOnBridge = false;
    protected bool isNextStage = false; //change later
    protected float interactRange;
    protected float heightOfBrick = 0.5f;

    protected NavMeshAgent agent;
    protected ColorEnum characterColorEnum;
    protected List<Brick> brickList = new();
    protected bool isRunning = false;
    public int score { protected set; get; } = 0;
    public bool isCanMoveForward { private set; get; } = true;
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        interactRange = 0.2f;
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
        if (stair.stairColor == ColorEnum.White && brickList.Count == 0)
            return false;
        return false;
    }
    private void ReachEndPoint()
    {
        Debug.Log("Finish");
        RemoveAllBrick();
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
        Brick brickClone = Instantiate(brickPrefab, transform);
        brickClone.transform.localPosition = Vector3.zero;
        brickClone.OnChangeColor(characterColorEnum);
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
        if (brickList.Count > 0)
        {
            for (int i = brickList.Count - 1; i <= 0; i++)
            {
                brickList[i].OnRemoveBox();
                brickList.RemoveAt(i);
            }
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
        score = 0;
    }

    public virtual void OnEndGame()
    {
        isEndGame = true;
    }

    public void OnResult(Transform transform,int rank)
    {
        agent.enabled = false;
        this.transform.position = transform.position;
        Debug.Log("transform.position_____" + transform.position);
        if (rank == 0)
            ChangeAnim(Constants.WIN_ANIM);
        else
            ChangeAnim(Constants.LOSE_ANIM);
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
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Constants.BRICK_TAG))
        {
            Brick brick = collider.GetComponent<Brick>();
            CheckBrick(brick);
        }
        else if (collider.CompareTag(Constants.STAGE_TAG))
        {
            if (isNextStage) return;
            currentStage++;
            LevelManager.Ins.CharacterMoveToNextStage(currentStage, characterColorEnum);
            isNextStage = true;
        }
        else if (collider.CompareTag(Constants.STAIR_TAG))
        {
            Stair stair = collider.GetComponent<Stair>();
            isCanMoveForward = IsCanMoveForward(stair);
            isOnBridge = true;
            CheckStair(stair);
        }
        else if (collider.CompareTag(Constants.WIN_TAG))
        {
            ReachEndPoint();
        }
        else
        {
            isOnBridge = false;
            isCanMoveForward = IsCanMoveForward();
        }
    }

}
