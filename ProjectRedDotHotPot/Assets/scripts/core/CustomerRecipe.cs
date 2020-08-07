using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "CustomerRecipe", menuName = "MagicJammer/Recipe", order = 1)]
public class CustomerRecipe : ScriptableObject
{
    public int OrderNumber;
    public Sprite CustomerSprite;
    public float RecipeMakingTime;
    public Recipe DishRecipe;

    public StoryData[] StoryData;
    public ChatData[] ChatData;
}