
public class MainMenu : UICanvas
{
    public void PlayButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }
}
