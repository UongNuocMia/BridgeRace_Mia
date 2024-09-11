using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public ColorEnum StairColor { private set; get; } = ColorEnum.White;


    public void OnChangeColor(ColorEnum color)
    {
        meshRenderer.material = GameManager.Ins.GetMaterial(color);
        StairColor = color;
    }
}
