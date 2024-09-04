using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public enum GameState { MainMenu, GamePlay, Finish, Setting }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private ColorDataSO colorDataSO;
    private Player player;

    private static GameState gameState = GameState.MainMenu;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;
    public List<Brick> brickinGroundList { private set; get; }
    public List<Character> characterList { private set; get; }
    public List<ColorEnum> randomColorList { private set; get; } = new();

    public Character winner;

    public int score { private set; get; } = 0;
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

        ChangeState(GameState.MainMenu);

        UIManager.Ins.OpenUI<MainMenu>();
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
        characterList = Spawner.Ins.characterList;
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].OnStartGame();
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
                OnStartGame();
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
        Dictionary<Character, int> charactersScore = new Dictionary<Character, int>();
        for (int i = 0; i < characterList.Count; i++)
        {
            charactersScore.Add(characterList[i], characterList[i].score);
            characterList[i].OnEndGame();
        }
        var top3Characters = charactersScore.
                                OrderByDescending(pair => pair.Value)
                                .Take(3).Select(pair => pair.Key).ToList();

        if(winner is Enemy)
        {
            UIManager.Ins.OpenUI<Lose>();
        }
        else
        {
            top3Characters[0] = player;
            UIManager.Ins.OpenUI<Win>();
        }
        for (int i = 0; i < top3Characters.Count; i++)
        {
            top3Characters[i].OnResult(LevelManager.Ins.rankTransformList[i],i);
        }
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
