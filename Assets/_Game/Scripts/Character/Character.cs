using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Character : GameUnit
{
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected Transform brickPosition;
    [SerializeField] private SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected float speed = 5;
    protected int currentStage = 0;
    protected bool isOnBridge = false;
    protected bool isNextStage = false;
    protected float interactRange;
    protected float heightOfBrick = 0.5f;
    protected ColorEnum characterColorEnum;
    protected RaycastHit hit;
    protected List<Brick> brickList = new();
    protected NavMeshAgent agent;
    
    public bool isCanMoveForward { private set; get; } = true;
    

    private void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        interactRange = 0.2f;
        agent = transform.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        RaycastScan();
    }

    protected virtual void RaycastScan()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            if (hit.transform.TryGetComponent(out Brick brick))
            {
                CheckBrick(brick);
            }
            else if (hit.transform.TryGetComponent(out EndLevel endLevel))
            {
                ReachEndPoint();
            }
            else if (hit.transform.TryGetComponent(out Stair stair))
            {
                CheckStair(stair);
                isOnBridge = true;
                isCanMoveForward = IsCanMoveForward(stair);
            }
            else if (hit.transform.TryGetComponent(out Stage stage))
            {
                if (isNextStage) return;
                currentStage++;
                LevelManager.Ins.CharacterMoveToNextStage(currentStage, characterColorEnum);
                isNextStage = true;
            }
        }
        else
        {
            isCanMoveForward = true;
            isOnBridge = false;
        }
    }

    public bool IsCanMoveForward(Stair stair)
    {
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
        RemoveAllBrick();
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
}
