using UnityEngine.UI;

public class Win : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        GameManager.Ins.OnNextLevel();
        Close(0);
    }

    public void TryAgainButton()
    {
        GameManager.Ins.OnPlayAgain();
        Close(0);
    }
    private void OnEnable()
    {
        score.text = GameManager.Ins.PlayerScore.ToString();
    }
}
