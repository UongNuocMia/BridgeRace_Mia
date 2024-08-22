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
    private static GameState gameState = GameState.MainMenu;
    private List<Brick> brickinGroundList;

    public List<Brick> BrickinGroundList => brickinGroundList;


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
    }


    public void HandlerScore()
    {
        score++;
    }

    public void OnStartGame()
    {
        LevelManager.Ins.OnLoadMap();
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


}
