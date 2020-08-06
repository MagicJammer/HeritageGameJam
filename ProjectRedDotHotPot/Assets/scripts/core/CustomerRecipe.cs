using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "CustomerRecipe", menuName = "MagicJammer/Recipe", order = 1)]
public class CustomerRecipe : ScriptableObject
{
    public int OrderNumber;
    //public bool OrderCompleted; //other way to do this hehe
    public Sprite CustomerSprite;
    public Recipe DishRecipe;

    public StoryData[] StoryData;
    public ChatData[] ChatData;
}