using UnityEngine;

public class Data : Singleton<Data>
{
    private string levelKey = "PlayerLevelKey";

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(levelKey, level);
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(levelKey);
    }
}
