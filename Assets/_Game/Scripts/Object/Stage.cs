using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageEnum
{
    Stage_1,
    Stage_2,
    Stage_3,
}

public class Stage : MonoBehaviour
{
    [SerializeField] private StageEnum stageEnum;

    public StageEnum GetStageEnum()
    {
        return stageEnum;
    }
}
