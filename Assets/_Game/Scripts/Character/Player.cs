using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private RaycastHit hit;
    private float interactRange;
    private float heightOfBrick;
    [SerializeField] private Brick brickPrefab;
    private List<Brick> brickList;

    private void OnInit()
    {
        interactRange = 2f;
    }

    private void RaycastScan()
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
        throw new NotImplementedException();
    }

    private void CheckBridge(Bridge bridge)
    {
        throw new NotImplementedException();
    }

    private void CheckBrick(Brick brick)
    {
        AddBrick();
    }

    private void AddBrick()
    {
        Brick brickClone = Instantiate(brickPrefab, transform);
        brickList.Add(brickClone);
        brickClone.transform.localPosition = Vector3.zero;
        GameManager.Ins.HandlerScore();
        HandlerBrickHeight();
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
