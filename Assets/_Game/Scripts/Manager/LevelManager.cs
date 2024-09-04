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

    public int characterNumb { private set; get; } = 0;
    public Transform endPointTransform { private set; get; }
    public List<Vector3> positionList { private set; get; }
    public List<Transform> rankTransformList { private set; get; }
    public List<Transform> spawnBrickPointList { private set; get; } = new();


    public void OnLoadMap()
    {
        SetUpMap();
    }

    private void SetUpMap()
    {
        characterNumb = 6;

        int currentLevel = 0; //change latter
        Level currentMap = Instantiate(levelList[currentLevel]);
        currentMap.transform.position = Vector3.zero;
        currentMap.OnInit();
        spawnBrickPointList = currentMap.GetSpawnBrickPointList();
        positionList = currentMap.GetSpawnCharacterPosition();
        endPointTransform = currentMap.GetEndPointTransform();
        rankTransformList = currentMap.GetListTransform();
        int indexOfPlayer = Random.Range(0, 6);
        Spawner.Ins.GenarateCharacter(positionList, indexOfPlayer);
        Spawner.Ins.GenarateBrick(spawnBrickPointList[0],ColorEnum.White);
    }

    public void CharacterMoveToNextStage(int characterStage,ColorEnum characterColorEnum)
    {
        Spawner.Ins.GenarateBrick(spawnBrickPointList[characterStage], characterColorEnum);
    }

}
