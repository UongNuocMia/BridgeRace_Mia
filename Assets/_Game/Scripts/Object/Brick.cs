using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private MeshRenderer colorRenderer;
    [SerializeField] private GameObject visualBrick;
    [SerializeField] private BoxCollider boxCollider;

    private void OnHideVisual(bool isShow) => visualBrick.SetActive(!isShow);
    private void OnHideCollision(bool isShow) => boxCollider.enabled = !isShow;

    public bool isShow() => visualBrick.gameObject.activeInHierarchy;

    public void OnChangeColor(int colorEnumIndex)
    {
        colorRenderer.material = GameManager.Ins.GetMaterial(ColorEnum.White); //change later
    }


    public void RandomColor()
    {
        int colorIndex = Random.Range(0, GameManager.Ins.GetMaterialList().Count);
        OnChangeColor(colorIndex);
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
