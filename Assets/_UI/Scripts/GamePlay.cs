
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
        GameManager.Ins.ChangeState(GameState.Setting);
    }
}
