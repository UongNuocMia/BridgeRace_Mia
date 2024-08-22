using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Character : GameUnit
{
    [SerializeField] protected Brick brickPrefab;
    protected float interactRange;
    protected float heightOfBrick;
    protected RaycastHit hit;
    protected List<Brick> brickList = new();
    protected NavMeshAgent agent;

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
            if (hit.transform.TryGetComponent(out Bridge bridge))
            {
                CheckBridge(bridge);
            }
            if (hit.transform.TryGetComponent(out EndLevel endLevel))
            {
                ReachEndPoint();
            }
        }
    }
    private void ReachEndPoint()
    {
        GameManager.Ins.ChangeState(GameState.Finish);
    }

    private void CheckBridge(Bridge bridge)
    {

    }

    private void CheckBrick(Brick brick)
    {
        brick.OnCollectBox();
        AddBrick();
    }

    protected virtual void AddBrick()
    {
        Brick brickClone = Instantiate(brickPrefab, transform);
        brickClone.transform.localPosition = Vector3.zero;
        brickList.Add(brickClone);
        HandlerBrickHeight();
    }

    protected virtual void RemoveBrick()
    {

    }

    protected virtual void RemoveAllBrick()
    {

    }
    private void HandlerBrickHeight()
    {
        if (brickList.Count == 0) return;
        Vector3 newHeightY = new();
        for (int i = 0; i < brickList.Count; i++)
        {
            newHeightY.y = Mathf.Abs(i * heightOfBrick);
            brickList[i].transform.localPosition = new Vector3(newHeightY.x, newHeightY.y);
        }
    }
}
