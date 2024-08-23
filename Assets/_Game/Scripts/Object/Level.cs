using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private List<Transform> spawnBrickPointList;
    [SerializeField] private Transform spawnCharacterPoint;
    [SerializeField] private List<GameUnit> brickList = new();
    private float brickLength;
    private float spacing = 4;
    private float halfSpacing = 2;
    private float halfLength = 0.5f;
    private int rows = 10;
    private int columns = 10;
    private int stage = 0;
    private int charNumb;
    public int CharNumb => charNumb;
    public void OnInit()
    {
        charNumb = Random.Range(4, 6);
        GetSpawnCharacterPosition();
        GenarateBrick();
    }

    public List<Vector3> GetSpawnCharacterPosition()
    {
        List<Vector3> transformList = new();
        int characterSpacing = 4;

        for (int i = 0; i < charNumb; i++)
        {
            transformList.Add(spawnCharacterPoint.position + new Vector3(i * characterSpacing, 0f, 0f));
        }
        return transformList;
    }

    private void GenarateBrick()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //Vector3 newPosition = spawnBrickPoint.position - new Vector3(i * spacing, spawnBrickPoint.position.y, j * spacing);
                Vector3 position = spawnBrickPointList[stage].position + new Vector3(
               (i - (rows - 1) / 2) * spacing + halfSpacing - halfLength, 0,
               (j - (columns - 1) / 2) * spacing + halfSpacing - halfLength);
                Brick brickClone = (Brick)SimplePool.Spawn(brickPrefab,
                position, spawnBrickPointList[stage].rotation);
                brickClone.RandomColor();
                brickList.Add(brickClone);
            }
        }
    }
    public List<GameUnit> GetBrickList()
    {
        return brickList;
    }
}
