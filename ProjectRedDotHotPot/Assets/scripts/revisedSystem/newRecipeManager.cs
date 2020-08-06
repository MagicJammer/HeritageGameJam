﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newRecipeManager : Singleton<newRecipeManager> {
    [Header("Recipe path")]
    public string RecipePath = "recipe_bags";
    //TODO to check if recipe is already completed
    Dictionary<FoodItemTag, CustomerRecipe> _recipeReference = new Dictionary<FoodItemTag, CustomerRecipe>();

    public Recipe _currentRecipe => _customerOrders[_currentOrderIdx].DishRecipe;
    List<CustomerRecipe> _customerOrders = new List<CustomerRecipe>();
    int _currentOrderIdx = 0;
    public ChatData[] _currentChatdatas => _customerOrders[_currentOrderIdx].ChatData;

    public event Action OnNewRecipe;
    public event Action<FoodItemTag> OnHoldInstructionUpdate;

    protected override void Awake() {
        base.Awake();
        CustomerRecipe[] rcps = Resources.LoadAll<CustomerRecipe>(RecipePath);
        rcps = rcps.OrderBy(x => x.OrderNumber).ToArray();

        foreach (CustomerRecipe r in rcps) {
            _recipeReference[r.DishRecipe.DishName] = r;
            _customerOrders.Add(r);
        }
    }

    private void Start() {
        rdUIManager.Seele.OnStoryTellingDone += OnStoryTellingDone;

    }

    protected override void OnDestroy() {
        base.OnDestroy();

        if (rdUIManager.Seele != null)
            rdUIManager.Seele.OnStoryTellingDone -= OnStoryTellingDone;
    }

    public void OrderServed() {
        StoryData[] story = _customerOrders[_currentOrderIdx].StoryData;
        rdUIManager.ShowStoryText(story);
    }

    void OnStoryTellingDone() {
        if (_currentOrderIdx >= _customerOrders.Count - 1) {
            print("finished game");
            return;
        }
        _currentOrderIdx++;
        NewRecipe();
    }

    //event to call all to replenish/change menu
    public void NewRecipe() {
        OnNewRecipe?.Invoke();
    }

    public static void UpdateInstruction(FoodItemTag foodInstruction) {
        Seele.OnHoldInstructionUpdate?.Invoke(foodInstruction);
    }
}