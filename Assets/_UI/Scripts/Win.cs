using UnityEngine.UI;

public class Win : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }
    private void OnEnable()
    {
        score.text = GameManager.Ins.score.ToString();
    }
}
