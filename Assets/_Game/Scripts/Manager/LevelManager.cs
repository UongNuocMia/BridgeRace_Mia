using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<Level> levelList;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Vector3 endPosition;
    private Player player;
    private List<GameUnit> listBrickOnGround;
    private List<Enemy> enemyList = new();
    public List<Enemy> EnemyList => enemyList;
    public List<GameUnit> ListBrickOnGround => listBrickOnGround;

    public void OnLoadMap()
    {
        SetUpMap();
    }

    private void SetUpMap()
    {
        int currentLevel = 0; //change latter
        Level currentMapLevel = Instantiate(levelList[currentLevel]);
        currentMapLevel.transform.position = Vector3.zero;
        currentMapLevel.OnInit();
        listBrickOnGround = currentMapLevel.GetBrickList();
        List<Vector3> positionList = currentMapLevel.GetSpawnCharacterPosition();
        int indexOfPlayer = Random.Range(0, currentMapLevel.CharNumb);
        for (int i = 0; i < currentMapLevel.CharNumb; i++)
        {
            if (indexOfPlayer == i)
                player = SetUpPlayer(positionList[i]);
            else
            {

                Enemy enemy = SetUpEnemy(positionList[i]);
                enemyList.Add(enemy);
            }
        }
    }
    private Enemy SetUpEnemy(Vector3 position)
    {
        Enemy enemy = (Enemy)SimplePool.Spawn(this.enemyPrefab, position, Quaternion.identity);
        return enemy;
    }
    private Player SetUpPlayer(Vector3 position)
    {
        Player player;
        return  player = Instantiate(this.playerPrefab, position, Quaternion.identity);
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
