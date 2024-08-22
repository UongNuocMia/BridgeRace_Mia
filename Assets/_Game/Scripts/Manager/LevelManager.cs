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
}


public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameUnit enemy;
    [SerializeField] private List<Level> levelList;
    private List<GameUnit> listBrickOnGround;
    public List<GameUnit> ListBrickOnGround => listBrickOnGround;

    public void OnLoadMap()
    {
        SetUpMap();
        SetUpEnemy();
    }

    private void SetUpMap()
    {
        int currentLevel = 0; //change latter
        Level currentMapLevel = Instantiate(levelList[currentLevel]);
        currentMapLevel.transform.position = Vector3.zero;
        listBrickOnGround = currentMapLevel.GetBrickList();
    }
    private void SetUpEnemy()
    {
        Vector3 position = new Vector3(0, 0, 0);
        SimplePool.Spawn(enemy, position, Quaternion.identity);
    }
}
