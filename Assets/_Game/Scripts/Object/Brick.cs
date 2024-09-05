using System.Collections;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private MeshRenderer colorRenderer;
    [SerializeField] private GameObject visualBrick;
    [SerializeField] private BoxCollider boxCollider;

    private ColorEnum brickColorEnum;

    public ColorEnum BrickColorEnum => brickColorEnum;

    private void OnHideVisual(bool isShow) => visualBrick.SetActive(!isShow);
    public void OnHideCollision(bool isShow) => boxCollider.enabled = !isShow;

    public bool isShow() => visualBrick.gameObject.activeInHierarchy;

    public void OnChangeColor(ColorEnum colorEnum)
    {
        brickColorEnum = colorEnum;
        colorRenderer.material = GameManager.Ins.GetMaterial(colorEnum);
    }

    public void OnRemoveBox()
    {
        OnHideCollision(true);
        OnHideVisual(true);
    }

    public void OnCollectBox()
    {
        OnHideCollision(true);
        OnHideVisual(true);
        StartCoroutine(BackToTheGround());
    }

    private IEnumerator BackToTheGround()
    {
        yield return new WaitForSeconds(6f);
        OnHideCollision(false);
        OnHideVisual(false);
    }
}
