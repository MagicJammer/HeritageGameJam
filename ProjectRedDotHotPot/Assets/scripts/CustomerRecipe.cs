using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "CustomerRecipe", menuName = "MagicJammer/CustomerRecipe", order = 1)]
public class CustomerRecipe : ScriptableObject
{
    public int OrderNumber;
    public Recipe DishRecipe;
    //public Image Picture;
    public StoryData[] StoryData;
    public ChatData[] ChatData;
}

[System.Serializable]
public struct StoryData 
{
    [TextArea(3, 5)]
    public string StoryLine;
    public float TextDelay;
}

[System.Serializable]
public struct ChatData {
    public string ChatLine;
    public float TextDelay;
}