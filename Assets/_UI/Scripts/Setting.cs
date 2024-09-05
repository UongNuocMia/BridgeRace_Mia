using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : UICanvas
{
    public void ContinueButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        Close(0);
    }
}
