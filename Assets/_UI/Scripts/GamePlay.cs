
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    [SerializeField] private Text scoreText;


    private void OnEnable()
    {
        scoreText.text = "Score: " +GameManager.Ins.playerScore.ToString();
    }

    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
        GameManager.Ins.ChangeState(GameState.Setting);
    }


    //public void WinButton()
    //{
    //    UIManager.Ins.OpenUI<Win>().score.text = Random.Range(100, 200).ToString();
    //    Close(0);
    //}

    //public void LoseButton()
    //{
    //    UIManager.Ins.OpenUI<Lose>().score.text = Random.Range(0, 100).ToString(); 
    //    Close(0);
    //}
}
