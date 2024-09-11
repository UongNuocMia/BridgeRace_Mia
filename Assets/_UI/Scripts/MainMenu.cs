
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : UICanvas
{
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private Text levelText;
    private void OnEnable()
    {
        levelText.text = "Level: " + (GameManager.Ins.Level + 1).ToString();
        if (GameManager.Ins.IsMaxLevel)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
    }
    

    public void PlayAgain()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameManager.Ins.IsPlayAgain(true);
    }
    public void PlayButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }
}
