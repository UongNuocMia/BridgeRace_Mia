using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Brick brickPrefab;

    private Player player;
    private List<Vector3> randomPositionList = new();

    public List<Enemy> enemyList { private set; get; } = new();
    public List<Character> characterList { private set; get; } = new();
    public Dictionary<Brick, ColorEnum> brickDict { private set; get; } = new();


    public void GenarateCharacter(List<Vector3> positionList, int indexOfPlayer)
    {
        for (int i = 0; i < LevelManager.Ins.characterNumb; i++)
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

    public void GenarateBrick(Transform startPoint, ColorEnum colorEnum)
    {
        randomPositionList = RandomPosition(startPoint);
        int allBrick = randomPositionList.Count;
        int maxBrickWithSameColor = allBrick / LevelManager.Ins.characterNumb;
        if (colorEnum == ColorEnum.White)
        {
            for (int i = 0; i < randomPositionList.Count; i++)
            {
                Brick brickClone = (Brick)SimplePool.Spawn(brickPrefab, randomPositionList[i], Quaternion.identity);
            RandomAgain:
                GameManager.Ins.SetRandomBrickColor(brickClone);
                int brickColorCount = brickDict.Count(brick => brick.Value == brickClone.BrickColorEnum);
                if (maxBrickWithSameColor == brickColorCount)
                {
                    goto RandomAgain;
                }
                brickDict.Add(brickClone, brickClone.BrickColorEnum);
            }

        }
        else
        {
            List<Brick> brickList = brickDict.
                Where(brick => brick.Value == colorEnum).
                Select(brick => brick.Key).ToList();
            Debug.Log("brickList count" + brickList.Count);
            for (int i = 0; i < brickList.Count; i++)
            {
                brickList[i].transform.position = randomPositionList[i];
                Debug.Log("brickList position" + brickList[i].transform.position);
                randomPositionList.RemoveAt(i);
            }
        }
    }

    public List<Vector3> RandomPosition(Transform startPoint)
    {
        List<Vector3> randomList = new();
        int rows = 5;
        int columns = 6;
        float spacing = 4.5f;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 newPosition = startPoint.position - new Vector3(-(i * spacing), 0, j * spacing);
                randomList.Add(newPosition);
            }
        }
        Utilities.Shuffle(randomList);
        return randomList;
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


}
