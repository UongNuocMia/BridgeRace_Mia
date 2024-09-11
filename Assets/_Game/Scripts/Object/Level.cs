using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private EndLevel winPlatform;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform spawnCharacterPoint;
    [SerializeField] private List<Transform> spawnBrickPointList;
    [SerializeField] private List<Transform> stageTransformList;
    public List<Vector3> GetSpawnCharacterPosition()
    {
        List<Vector3> transformList = new();
        int characterSpacing = 4;

        for (int i = 0; i < LevelManager.Ins.CharacterNumb; i++)
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

    public List<Transform> GetTransformList()
    {
        return winPlatform.GetTransformList();
    }

    public List<MeshRenderer> GetMeshRenderersList()
    {
        return winPlatform.GetMeshRenderersList();
    }

    public List<Transform> GetStageTransformList()
    {
        return stageTransformList;
    }
}
