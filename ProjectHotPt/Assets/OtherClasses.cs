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
    public string WName;
    public WorkStationType Type;

    public bool IsOccupied;
    public bool IsAutomated;
}

public enum WorkStationType {ChopBoard, Cooker, FryingPan}