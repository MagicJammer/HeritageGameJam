using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public string IName;
    public IngredientType Type;

    public bool IsPick;
    public bool CanChop;
    public bool CanCook;

    public bool IsChopped;
}

public enum IngredientType {Beef, Pork, Chicken, Tomato}

[System.Serializable]
public class WorkStation
{
    [SerializeField] string WName;
    public WorkStationType Type;
    [SerializeField] bool IsAutomated;

    public bool IsOccupied;
    
    public bool IsLoaded;

    public int MaximumIngredient;

    public List<Ingredient> CurrentIngredient = new List<Ingredient>();

    public void ClearStation ()
    {
        IsLoaded = false;
        CurrentIngredient.Clear();
    }
}

public enum WorkStationType { Stove, FryingPan, SoupBoiler, Oven, ChopBoard }

[System.Serializable]
public class CookingRecipe
{
    public string RName;
    public List<Ingredient> IngredientLists;
    public bool IsFinal;
    public float CookingTime;

    public CookAction Action;
    public CookDish CompleteDish;
    public Ingredient CompleteIngredient;

    CookingRecipe ()
    {
        if (IsFinal)
            CompleteIngredient = null;
        else
            CompleteDish = CookDish.None;
    }
}

public class Dishes
{

}

public enum CookDish {None, Fail, BeefStew }
public enum CookAction { Chop, Boil, Fry, Stir, Bake }
