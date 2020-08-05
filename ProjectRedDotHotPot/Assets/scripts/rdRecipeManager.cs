using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rdRecipeManager : Singleton<rdRecipeManager>
{
    public Recipe CurrentRecipe=> CustomerOrders[CurrentOrder].DishRecipe;
    public Dictionary<FoodItemTag, rdCollectStation> IngredientStations = new Dictionary<FoodItemTag, rdCollectStation>();
    public Dictionary<WorkstationTag, rdWorkstation> KitchenStations = new Dictionary<WorkstationTag, rdWorkstation>();
    public string RecipeFolder = "recipe_bags";
    public int CurrentOrder = 0;
    CustomerRecipe[] CustomerOrders;
    public int DialogueIdx;
    public string[] DialogueLines;
    public Dictionary<FoodItemTag, WorkstationTag> RemainingIngredients = new Dictionary<FoodItemTag, WorkstationTag>();
    public ChatData[] _currentChatdatas => CustomerOrders[CurrentOrder].ChatData;
    protected override void Awake()
    {
        base.Awake();
        CustomerRecipe[] cr = Resources.LoadAll<CustomerRecipe>(RecipeFolder);
        int rcount = cr.Length;
        CustomerRecipe[] arr = new CustomerRecipe[rcount];
        foreach(CustomerRecipe o in cr)
        {
            int i = o.OrderNumber;
            arr[i] = o;
        }
        CustomerOrders = arr;

        rdUIManager.Seele.OnStoryTellingDone += OnStoryTellingDone;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(rdUIManager.Seele != null)
        rdUIManager.Seele.OnStoryTellingDone -= OnStoryTellingDone;
    }
    void Start()
    {
        OrderCor.FirstOrder(this);
            NextRecipe();
    }
    public void NextRecipe()
    {
        foreach (RecipeInstruction instruction in CurrentRecipe.Instructions)
        {
            WorkstationTag w = instruction.Workstation;
            foreach (FoodItemTag tag in instruction.Ingredients)
            {
                //RemainingIngredients.Add(tag,w);
                RemainingIngredients[tag] = w;
            }
            KitchenStations[w].SetRecipe(instruction);            
        }
        foreach (FoodItemTag t in RemainingIngredients.Keys)
            IngredientStations[t].SetRecipe(t);
    }
    public void OnPickup(FoodItemTag tag)
    {
        WorkstationTag w = RemainingIngredients[tag];
        rdWorkstation station = KitchenStations[w];
        station.Status = StationStatus.Ready;
    }
    public void OrderServed()
    {
        CurrentOrder++;
        if (CurrentOrder < CustomerOrders.Length)
        {
            foreach (rdWorkstation s in KitchenStations.Values)
                s.ResetRecipe();

            //DialogueLines = CustomerOrders[CurrentOrder - 1].StoryLines;
            //call the event on the uimanager to play
            StoryData[] currentStories = CustomerOrders[CurrentOrder - 1].StoryData;
            rdUIManager.ShowStoryText(currentStories);

        }
        else
            Debug.Log("Rating");
    }
    public void Chat()
    {
        if(DialogueIdx<DialogueLines.Length)
        {
            Debug.Log(DialogueIdx);
            DialogueIdx++;
        }
    }

    void OnStoryTellingDone() 
    {
        NextRecipe();
    }
}