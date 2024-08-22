using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private Transform spawnBrickPoint;
    [SerializeField] private List<GameUnit> brickList = new();
    private float brickLength;
    private float spacing = 2;
    private float halfSpacing = 1;
    private float halfLength = 0.5f;
    private int rows = 5;
    private int columns = 5;
    private void Start()
    {
        GenarateBrick();
    }

    private void GenarateBrick()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //Vector3 newPosition = spawnBrickPoint.position - new Vector3(i * spacing, spawnBrickPoint.position.y, j * spacing);
                Vector3 position = spawnBrickPoint.position + new Vector3(
               (i - (rows - 1) / 2) * spacing + halfSpacing - halfLength, 0,
               (j - (columns - 1) / 2) * spacing + halfSpacing - halfLength);
                GameUnit brickClone = SimplePool.Spawn(brickPrefab,
                position, spawnBrickPoint.rotation);
                brickList.Add(brickClone);
            }
        }
    }
    public List<GameUnit> GetBrickList()
    {
        return brickList;
    }
}
