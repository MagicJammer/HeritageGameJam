using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rdRecipeManager : MonoBehaviour
{
    public Recipe CurrentRecipe;
    public Dictionary<FoodItemTag, rdCollectStation> IngredientStations = new Dictionary<FoodItemTag, rdCollectStation>();
    public Dictionary<WorkstationTag, rdWorkstation> KitchenStations = new Dictionary<WorkstationTag, rdWorkstation>();
    public string RecipeFolder = "recipe_bags";
    public int CurrentOrder = 0;
    CustomerRecipe[] CustomerOrders;
    //List<CustomerRecipe> CustomerOrders = new List<CustomerRecipe>();
    public Dictionary<FoodItemTag, WorkstationTag> RemainingIngredients = new Dictionary<FoodItemTag, WorkstationTag>();
    private void Awake()
    {
        CustomerRecipe[] cr = Resources.LoadAll<CustomerRecipe>(RecipeFolder);
        //CustomerOrders.AddRange(cr);
        int rcount = cr.Length;
        CustomerRecipe[] arr = new CustomerRecipe[rcount];
        foreach(CustomerRecipe o in cr)
        {
            int i = o.OrderNumber;
            arr[i] = o;
        }
        CustomerOrders = arr;
    }
    void Start()
    {
        CurrentOrder++;
        if (CurrentOrder <= CustomerOrders.Length)
            NextRecipe();
    }
    public void NextRecipe()
    {
        foreach (RecipeInstruction instruction in CurrentRecipe.Instructions)
        {
            WorkstationTag w = instruction.Workstation;
            foreach (FoodItemTag tag in instruction.Ingredients)
            {
                RemainingIngredients.Add(tag,w);
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
        station.Status = WorkstationStatus.Ready;
    }
    //public void OnWorkDone()
    //{
    //    foreach (rdWorkstation station in KitchenStations.Values)
    //        station.Status = WorkstationStatus.Ready;
    //}
    public void OrderServed()
    {
        CurrentOrder++;
        if (CurrentOrder <= CustomerOrders.Length)
        {
            foreach (rdWorkstation s in KitchenStations.Values)
                s.ResetRecipe();
            NextRecipe();
        }
        else
            Debug.Log("Rating");
    }
}