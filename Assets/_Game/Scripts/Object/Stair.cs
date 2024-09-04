using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public ColorEnum stairColor { private set; get; } = ColorEnum.White;


    public void OnChangeColor(ColorEnum color)
    {
        meshRenderer.material = GameManager.Ins.GetMaterial(color);
        stairColor = color;
    }
}
