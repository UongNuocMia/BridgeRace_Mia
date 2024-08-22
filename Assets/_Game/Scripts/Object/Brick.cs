using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private MeshRenderer colorMaterial;
    [SerializeField] private GameObject visualBrick;
    [SerializeField] private BoxCollider boxCollider;

    private void OnHideVisual(bool isShow) => visualBrick.SetActive(!isShow);
    private void OnHideCollision(bool isShow) => boxCollider.enabled = !isShow;

    public bool isShow() => visualBrick.gameObject.activeInHierarchy;

    public void OnChangeColor(ColorEnum colorEnum)
    {
        //colorMaterial.material = GameManager.Ins.GetColorByEnum(colorEnum);
    }

    public void OnCollectBox()
    {
        OnHideCollision(true);
        OnHideVisual(true);
        StartCoroutine(BackToTheGround());
    }

    private IEnumerator BackToTheGround()
    {
        yield return new WaitForSeconds(5f);
        OnHideCollision(false);
        OnHideVisual(false);
    }
}
