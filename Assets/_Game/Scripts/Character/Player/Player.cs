using System.Collections.Generic;
using UnityEngine;
public class Player : Character
{
    private PlayerMovement playerMovement;

    protected override void OnInit()
    {
        base.OnInit();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {
            if (playerMovement.IsRunning())
                ChangeAnim(Constants.RUN_ANIM);
            else
                ChangeAnim(Constants.IDLE_ANIM);
        }
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }

    public List<Brick> GetBrickList()
    {
        return brickList;
    }
}
