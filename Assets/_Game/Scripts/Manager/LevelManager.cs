using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<Level> levelList;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Brick brickPrefab;

    private int stage = 0;
    private Player player;
    private List<Transform> spawnBrickPointList = new();
    public int characterNumb { private set; get; } = 0;
    public List<Enemy> enemyList { private set; get; } = new();
    public List<Character> characterList { private set; get; } = new();
    public Transform endPointTransform { private set; get; }
    public Dictionary<Brick, ColorEnum> brickDict { private set; get; } = new();


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
        List<Vector3> positionList = currentMap.GetSpawnCharacterPosition();
        endPointTransform = currentMap.GetEndPointTransform();
        int indexOfPlayer = Random.Range(0, characterNumb);
        GenarateCharacter(positionList, indexOfPlayer);
        GenarateBrick();
    }

    private void GenarateCharacter(List<Vector3> positionList, int indexOfPlayer)
    {
        for (int i = 0; i < characterNumb; i++)
        {
            Character character;
            if (indexOfPlayer == i)
            {
                character = SetUpPlayer(positionList[i]);
                player = (Player)character;
            }
            else
            {
                character = SetUpEnemy(positionList[i]);
                enemyList.Add((Enemy)character);
            }
            characterList.Add(character);
        }
        GameManager.Ins.SetRandomCharacterColor();
    }

    private Enemy SetUpEnemy(Vector3 position)
    {
        Enemy enemy = (Enemy)SimplePool.Spawn(this.enemyPrefab, position, Quaternion.identity);
        return enemy;
    }
    private Player SetUpPlayer(Vector3 position)
    {
        Player player;
        return player = Instantiate(this.playerPrefab, position, Quaternion.identity);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public List<Enemy> GetListEnemy()
    {
        return enemyList;
    }


    private void GenarateBrick()
    {
        int rows = 5;
        int columns = 5;
        int spacing = 5;
        int allBrick = rows * columns;
        int maxBrickWithSameColor = allBrick / characterNumb;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 newPosition = spawnBrickPointList[stage].position - new Vector3(-(i* spacing), 0, j* spacing);
                Brick brickClone = (Brick)SimplePool.Spawn(brickPrefab,
                newPosition, spawnBrickPointList[stage].rotation);
            RandomAgain:
                GameManager.Ins.SetRandomBrickColor(brickClone);
                int brickColorCount = brickDict.Count(brick => brick.Value == brickClone.BrickColorEnum);
                //if (maxBrickWithSameColor == brickColorCount)
                //{
                //    goto RandomAgain;
                //}
                brickDict.Add(brickClone, brickClone.BrickColorEnum);
            }
        }
    }
}
