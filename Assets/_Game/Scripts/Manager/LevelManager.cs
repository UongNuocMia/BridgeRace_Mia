using System.Collections.Generic;
using UnityEngine;

public enum ColorEnum
{
    White, //none
    Red,
    Purple,
    Green,
    Blue,
    Yellow,
    Pink,
    Black,
    Grey,
}


public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Level> levelList;
    [SerializeField] private Vector3 endPosition;

    private Level currentMap = null;

    public int totalLevelNumb => levelList.Count;
    public int characterNumb { private set; get; } = 0;
    public Transform endPointTransform { private set; get; }
    public List<Vector3> positionList { private set; get; }
    public List<Transform> rankTransformList { private set; get; }
    public List<Transform> spawnBrickPointList { private set; get; } = new();
    public List<MeshRenderer> meshRenderersList { private set; get; }


    public void OnLoadMap()
    {
        SetUpMap();
        GenarateObjects();
    }

    private void SetUpMap()
    {
        characterNumb = 6;

        DestroyMap();
        int currentLevel = GameManager.Ins.Level;
        currentMap = Instantiate(levelList[currentLevel]);
        currentMap.transform.position = Vector3.zero;
        spawnBrickPointList = currentMap.GetSpawnBrickPointList();
        positionList = currentMap.GetSpawnCharacterPosition();
        endPointTransform = currentMap.GetEndPointTransform();
        rankTransformList = currentMap.GetTransformList();
        meshRenderersList = currentMap.GetMeshRenderersList();
    }

    private void DestroyMap()
    {
        Debug.Log("currentMap____"+currentMap);
        if (currentMap == null)
            return;
        Destroy(currentMap.gameObject);
    }

    private void GenarateObjects()
    {
        int indexOfPlayer = Random.Range(0, 6);
        Spawner.Ins.GenarateCharacter(positionList, indexOfPlayer);
        Spawner.Ins.GenarateBrick(spawnBrickPointList[0], ColorEnum.White);
    }

    public void CharacterMoveToNextStage(int characterStage,ColorEnum characterColorEnum)
    {
        Spawner.Ins.GenarateBrick(spawnBrickPointList[characterStage], characterColorEnum);
    }

}
