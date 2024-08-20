using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ColorList")]
public class ColorDataSO : ScriptableObject
{
    public List<Material> materialsList;

    public Material GetMaterials(ColorEnum colorEnum) => materialsList[(int)colorEnum];
}
