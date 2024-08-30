using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;



public enum GameState { MainMenu, GamePlay, Finish, Setting }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private ColorDataSO colorDataSO;
    private List<Enemy> enemyList; 
    private Player player;

    private static GameState gameState = GameState.MainMenu;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;
    public List<Brick> brickinGroundList { private set; get; }
    public List<Character> characterList { private set; get; }
    public List<ColorEnum> randomColorList { private set; get; } = new();


    private int score;
    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        //csv.OnInit();
        //userData?.OnInitData();

        //ChangeState(GameState.MainMenu);

        UIManager.Ins.OpenUI<MianMenu>();
        PrepareLevel();
    }

    public void HandlerScore()
    {
        score++;
    }

    public void PrepareLevel()
    {
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.FindPlayer(player.transform);
    }
    public void OnStartGame()
    {
        enemyList = Spawner.Ins.enemyList;
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].ChangeState(new CollectBrickState());
        }
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.GamePlay:
                break;
            case GameState.Finish:
                OnFinish();
                break;
            case GameState.Setting:
                break;
            default:
                break;
        }
    }

    private void OnFinish()
    {
    }

    public static bool IsState(GameState state) => gameState == state;

    public Material GetMaterial(ColorEnum colorEnum)
    {
        return colorDataSO.GetMaterials(colorEnum);
    }
    public List<Material> GetMaterialList()
    {
        return colorDataSO.materialsList;
    }

    public void SetRandomCharacterColor()
    {
        characterList = Spawner.Ins.characterList;
        while (randomColorList.Count != characterList.Count)
        {
            ColorEnum colorEnum = (ColorEnum)Random.Range(1, System.Enum.GetValues(typeof(ColorEnum)).Length);
            if (!randomColorList.Contains(colorEnum))
            {
                characterList[randomColorList.Count].OnChangeColor(GetMaterial(colorEnum), colorEnum);
                randomColorList.Add(colorEnum);
            }
        }

    }

    public void SetRandomBrickColor(Brick brick)
    {
        int id = Random.Range(0, randomColorList.Count);
        brick.OnChangeColor(randomColorList[id]);
    }
}
