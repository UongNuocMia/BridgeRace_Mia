using UnityEngine.UI;

public class Lose : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }
    private void OnEnable()
    {
        score.text = GameManager.Ins.playerScore.ToString();
    }
}