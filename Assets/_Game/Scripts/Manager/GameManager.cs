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

    public Character winner;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;

    public int playerScore { private set; get; }
    public List<Character> characterList { private set; get; }
    public List<ColorEnum> randomColorList { private set; get; } = new();
    public int Level { private set; get; }

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
        UIManager.Ins.OpenUI<MainMenu>();
        Level = Data.Ins.GetLevel();
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.FindCharacter(player.transform);
        characterList = Spawner.Ins.characterList;
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].OnPrepareGame();
        }
    }
    private void OnStartGame()
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].OnStartGame();
        }
    }
    public void OnPlayAgain()
    {
        ChangeState(GameState.MainMenu);
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].OnEndGame();
        }
    }

    private void OnSetting()
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].OnSetting();
        }    
    }

    private void OnFinish()
    {
        List<Character> top3Characters = GetTop3Characters();
        playerScore = player.score;
        UIManager.Ins.CloseUI<GamePlay>();
        if (winner is Enemy)
        {
            UIManager.Ins.OpenUI<Lose>();
        }
        else
        {
            UIManager.Ins.OpenUI<Win>();
            Level = Level += 1;
            if (Level >= LevelManager.Ins.totalLevelNumb)
            {
                Level = 0; // cần thêm phần thông báo hết level
            }
            Data.Ins.SetLevel(Level);
        }
        for (int i = 0; i < top3Characters.Count; i++)
        {
            top3Characters[i].OnResult(LevelManager.Ins.rankTransformList[i], i);
            LevelManager.Ins.meshRenderersList[i].material = GetMaterial(top3Characters[i].characterColorEnum);
        }
        CameraFollow.FindCharacter(top3Characters[0].transform);
    }

    private List<Character> GetTop3Characters()
    {
        List<Character> top3Characters = new List<Character>();
        Dictionary<Character, int> charactersScore = new Dictionary<Character, int>();
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i] == winner)
                continue;
            charactersScore.Add(characterList[i], characterList[i].score);
            characterList[i].OnEndGame();
        }
        top3Characters.Add(winner);
        winner.OnEndGame();
        top3Characters.AddRange( charactersScore.
                                OrderByDescending(pair => pair.Value)
                                .Take(2).Select(pair => pair.Key));
        return top3Characters;
    }

    public Material GetMaterial(ColorEnum colorEnum)
    {
        return colorDataSO.GetMaterials(colorEnum);
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
