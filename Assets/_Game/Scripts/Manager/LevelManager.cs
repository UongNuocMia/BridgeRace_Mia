using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public int CharacterNumb { private set; get; } = 0;
    public Transform EndPointTransform { private set; get; }
    public List<Vector3> PositionList { private set; get; }
    public List<Character> CharacterList { private set; get; }
    public List<Transform> RankTransformList { private set; get; }
    public List<Transform> SpawnBrickPointList { private set; get; } = new();
    public List<MeshRenderer> MeshRenderersList { private set; get; }


    public void OnLoadMap()
    {
        SetUpMap();
        GenarateObjects();
    }

    private void SetUpMap()
    {
        CharacterNumb = 6;

        DestroyMap();
        int currentLevel = GameManager.Ins.Level;
        currentMap = Instantiate(levelList[currentLevel]);
        currentMap.transform.position = Vector3.zero;
        SpawnBrickPointList = currentMap.GetSpawnBrickPointList();
        PositionList = currentMap.GetSpawnCharacterPosition();
        EndPointTransform = currentMap.GetEndPointTransform();
        RankTransformList = currentMap.GetTransformList();
        MeshRenderersList = currentMap.GetMeshRenderersList();
    }

    private void DestroyMap()
    {
        if (currentMap == null)
            return;
        Destroy(currentMap.gameObject);
    }

    private void GenarateObjects()
    {
        int indexOfPlayer = Random.Range(0, 6);
        Spawner.Ins.GenarateCharacter(PositionList, indexOfPlayer);
        Spawner.Ins.GenarateBrick(SpawnBrickPointList[0], ColorEnum.White);
        CharacterList = Spawner.Ins.CharacterList;
    }

    public void CharacterMoveToNextStage(int characterStage,ColorEnum characterColorEnum)
    {
        Spawner.Ins.GenarateBrick(SpawnBrickPointList[characterStage], characterColorEnum);
    }

    public void CharactersOnStartGame()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnStartGame();
        }
    }

    public void CharactersOnEndGame()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnEndGame();
        }
    }

    public void CharactersOnSetting()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnSetting();
        }
    }

    public void CharacterOnPrepare()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnPrepareGame();
        }
    }

    public List<Character> GetTop3Characters()
    {
        List<Character> top3Characters = new List<Character>();
        Dictionary<Character, int> charactersScore = new Dictionary<Character, int>();
        for (int i = 0; i < CharacterList.Count; i++)
        {
            if (CharacterList[i] == GameManager.Ins.Winner)
                continue;
            charactersScore.Add(CharacterList[i], CharacterList[i].Score);
        }
        top3Characters.Add(GameManager.Ins.Winner);
        top3Characters.AddRange(charactersScore.
                                OrderByDescending(pair => pair.Value)
                                .Take(2).Select(pair => pair.Key));
        return top3Characters;
    }
}
