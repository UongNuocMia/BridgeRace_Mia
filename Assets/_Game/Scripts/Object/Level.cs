using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform spawnCharacterPoint;
    [SerializeField] private List<Transform> spawnBrickPointList;
    [SerializeField] private EndLevel winRank;
    public void OnInit()
    {
        GetSpawnCharacterPosition();
    }

    public List<Vector3> GetSpawnCharacterPosition()
    {
        List<Vector3> transformList = new();
        int characterSpacing = 4;

        for (int i = 0; i < LevelManager.Ins.characterNumb; i++)
        {
            transformList.Add(spawnCharacterPoint.position + new Vector3(i * characterSpacing, 0f, 0f));
        }
        return transformList;
    }

    public List<Transform> GetSpawnBrickPointList()
    {
        return spawnBrickPointList;
    }
    
    public Transform GetEndPointTransform()
    {
        return endPoint;
    }

    public List<Transform> GetListTransform()
    {
        return winRank.GetListTransform();
    }
}
