using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            //tf = tf ?? gameObject.transform;
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    public PoolType poolType;

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        tf.position = pos;
        tf.rotation = rot;
    }

    public void SetPosition(Vector3 pos)
    {
        tf.position = pos;
    }
}