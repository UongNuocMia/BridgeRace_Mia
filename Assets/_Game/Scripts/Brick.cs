using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private MeshRenderer colorMaterial;
    [SerializeField] private GameObject visualBrick;
    [SerializeField] private BoxCollider boxCollider;

    public void OnHideVisual(bool isShow) => visualBrick.SetActive(!isShow);
    public void OnHideCollision(bool isShow) => boxCollider.enabled = !isShow;

    public void OnChangeColor(ColorEnum colorEnum)
    {
        //colorMaterial.material = GameManager.Ins.GetColorByEnum(colorEnum);
    }
}
