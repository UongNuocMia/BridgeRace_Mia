using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Brick brickPrefab;

    private Player player;
    private List<Vector3> randomPositionList = new();
    private List<Vector3> positionBrickList = new();

    public List<Enemy> EnemyList { private set; get; } = new();
    public List<Character> CharacterList { private set; get; } = new();
    public Dictionary<Brick, ColorEnum> BrickDict { private set; get; } = new();

    public void GenarateCharacter(List<Vector3> positionList, int indexOfPlayer)
    {
        if(CharacterList.Count > 0)
        {
            for (int i = 0; i < CharacterList.Count; i++)
            {
                CharacterList[i].SetPositionAndRotation(positionList[i], Quaternion.Euler(Vector3.zero));
            }
        }
        else
        {
            for (int i = 0; i < LevelManager.Ins.CharacterNumb; i++)
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
                    EnemyList.Add((Enemy)character);
                }
                CharacterList.Add(character);
            }
            GameManager.Ins.SetRandomCharacterColor();
        }
    }

    public void GenarateBrick(Transform startPoint, ColorEnum colorEnum)
    {
        randomPositionList = RandomPosition(startPoint);
        int allBrick = randomPositionList.Count;
        int maxBrickWithSameColor = allBrick / LevelManager.Ins.CharacterNumb;
        if (colorEnum == ColorEnum.None && BrickDict.Count > 0)
        {
            positionBrickList.Clear();
            List<Brick> brickList = BrickDict.Select(brick => brick.Key).ToList();
            for (int i = 0; i < randomPositionList.Count; i++)
            {
                brickList[i].SetPosition(randomPositionList[i]);
            }
            return;
        }
        if (colorEnum == ColorEnum.None)
        {
            for (int i = 0; i < randomPositionList.Count; i++)
            {
                Brick brickClone = (Brick)SimplePool.Spawn(brickPrefab, randomPositionList[i], Quaternion.identity);
            RandomAgain:
                GameManager.Ins.SetRandomBrickColor(brickClone);
                int brickColorCount = BrickDict.Count(brick => brick.Value == brickClone.BrickColorEnum);
                if (maxBrickWithSameColor == brickColorCount)
                {
                    goto RandomAgain;
                }
                BrickDict.Add(brickClone, brickClone.BrickColorEnum);
            }

        }
        else
        {
            List<Brick> brickList = BrickDict.
                Where(brick => brick.Value == colorEnum).
                Select(brick => brick.Key).ToList();
            int numbBrickSetSuccess = 0;
            for (int i = 0; i < randomPositionList.Count; i++)
            {
                if (positionBrickList.Contains(randomPositionList[i]))
                    continue;
                if (numbBrickSetSuccess >= brickList.Count)
                    break;
                brickList[numbBrickSetSuccess].SetPosition(randomPositionList[i]);
                numbBrickSetSuccess++;
                positionBrickList.Add(randomPositionList[i]);
            }
        }
    }

    public List<Vector3> RandomPosition(Transform startPoint)
    {
        List<Vector3> randomList = new();
        int rows = 12;
        int columns = 9;
        float spacing = 2f;
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
        Player player = (Player)SimplePool.Spawn(this.playerPrefab, position, Quaternion.identity);
        return player;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public List<Enemy> GetListEnemy()
    {
        return EnemyList;
    }
}
