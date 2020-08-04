using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "CustomerRecipe", menuName = "MagicJammer/CustomerRecipe", order = 1)]
public class CustomerRecipe : ScriptableObject
{
    public int OrderNumber;
    public Recipe DishRecipe;
    //public Image Picture;
    public string[] Story;
}