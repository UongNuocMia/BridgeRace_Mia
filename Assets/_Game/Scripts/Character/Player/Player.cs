using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
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
