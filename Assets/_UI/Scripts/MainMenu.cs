
public class MainMenu : UICanvas
{
    public void PlayButton()
    {
        //UIManager.Ins.OpenUI<GamePlay>();
        GameManager.Ins.ChangeState(GameState.GamePlay);
        Close(0);
    }
}
