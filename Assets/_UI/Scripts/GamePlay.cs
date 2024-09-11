
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
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
