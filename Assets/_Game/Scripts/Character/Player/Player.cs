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
            IsRunningAnim(playerMovement.IsRunning());
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
