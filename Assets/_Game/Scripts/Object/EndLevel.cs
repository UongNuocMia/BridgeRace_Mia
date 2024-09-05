using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> rankTransform;
    [SerializeField] private List<MeshRenderer> meshRenderersList;

    public List<Transform> GetTransformList()
    {
        return rankTransform;
    }

    public List<MeshRenderer> GetMeshRenderersList()
    {
        return meshRenderersList;
    }
}
