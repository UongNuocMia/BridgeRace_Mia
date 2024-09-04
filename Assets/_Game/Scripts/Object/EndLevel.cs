using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> rankTransform;

    public List<Transform> GetListTransform()
    {
        return rankTransform;
    }
}
