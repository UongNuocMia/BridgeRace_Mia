using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Setting }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private ColorDataSO colorDataSO;

    private Player player;
    public Character Winner;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;

    public int Level { private set; get; }
    public int PlayerScore { private set; get; }
    public bool IsMaxLevel { private set; get; }
    public List<ColorEnum> RandomColorList { private set; get; } = new();

    private static GameState gameState = GameState.MainMenu;
    public static bool IsState(GameState state) => gameState == state;

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

        
    }
    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.MainMenu:
                PrepareLevel();
                break;
            case GameState.GamePlay:
                OnStartGame();
                break;
            case GameState.Finish:
                OnFinish();
                break;
            case GameState.Setting:
                OnSetting();
                break;
            default:
                break;
        }
    }

    private void PrepareLevel()
    {
        Level = Data.Ins.GetLevel();
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.FindCharacter(player.transform);
        LevelManager.Ins.CharacterOnPrepare();
    }
    private void OnStartGame()
    {
        LevelManager.Ins.CharactersOnStartGame();
    }
    public void OnPlayAgain()
    {
        ChangeState(GameState.MainMenu);
        LevelManager.Ins.CharactersOnEndGame();
    }

    private void OnSetting()
    {
        LevelManager.Ins.CharactersOnSetting();
    }

    private void OnFinish()
    {
        LevelManager.Ins.CharactersOnEndGame();
        List<Character> top3Characters = LevelManager.Ins.GetTop3Characters();
        PlayerScore = player.Score;
        UIManager.Ins.CloseUI<GamePlay>();
        if (Winner is Enemy)
        {
            UIManager.Ins.OpenUI<Lose>();
        }
        else
        {
            UIManager.Ins.OpenUI<Win>();
        }
        for (int i = 0; i < top3Characters.Count; i++)
        {
            top3Characters[i].OnResult(LevelManager.Ins.RankTransformList[i], i);
            LevelManager.Ins.MeshRenderersList[i].material = GetMaterial(top3Characters[i].CharacterColorEnum);
        }
        CameraFollow.FindCharacter(top3Characters[0].transform);
    }

    public void OnNextLevel()
    {
        Level = Level += 1;
        if (Level >= LevelManager.Ins.totalLevelNumb)
        {
            IsMaxLevel = true;
            Level = 0;
        }
        Data.Ins.SetLevel(Level);
        ChangeState(GameState.MainMenu);
    }

   

    public Material GetMaterial(ColorEnum colorEnum)
    {
        return colorDataSO.GetMaterials(colorEnum);
    }

    public void SetRandomCharacterColor()
    {
        while (RandomColorList.Count != Spawner.Ins.CharacterList.Count)
        {
            ColorEnum colorEnum = (ColorEnum)Random.Range(2, System.Enum.GetValues(typeof(ColorEnum)).Length);
            if (!RandomColorList.Contains(colorEnum))
            {
                Spawner.Ins.CharacterList[RandomColorList.Count].OnChangeColor(GetMaterial(colorEnum), colorEnum);
                RandomColorList.Add(colorEnum);
            }
        }
    }

    public void SetRandomBrickColor(Brick brick)
    {
        int id = Random.Range(0, RandomColorList.Count);
        brick.OnChangeColor(RandomColorList[id]);
    }

    public void IsPlayAgain(bool isPlayAgain) => IsMaxLevel = !isPlayAgain;
}
