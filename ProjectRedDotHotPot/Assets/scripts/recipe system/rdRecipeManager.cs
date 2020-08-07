using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rdRecipeManager : Singleton<rdRecipeManager> {
    [Header("Recipe path")]
    public string RecipePath = "recipe_bags";
    //TODO to check if recipe is already completed
    Dictionary<FoodItemTag, CustomerRecipe> _recipeReference = new Dictionary<FoodItemTag, CustomerRecipe>();
    public AudioAmbienceDynamic CrowdSoundClip;
    AudioSource _CrowdSound;
    public Recipe _currentRecipe => _customerOrders[_currentOrderIdx].DishRecipe;
    List<CustomerRecipe> _customerOrders = new List<CustomerRecipe>();
    int _currentOrderIdx = 0;
    public ChatData[] _currentChatdatas => _customerOrders[_currentOrderIdx].ChatData;
    public CustomerRecipe _currentCustomer => _customerOrders[_currentOrderIdx];

    public bool JustStarted = true;

    //waaa
    public int _allOrders => _customerOrders.Count;

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
        _CrowdSound = GetComponent<AudioSource>();

        NewRecipe();
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        if (rdUIManager.Seele != null)
            rdUIManager.Seele.OnStoryTellingDone -= OnStoryTellingDone;
    }

    public void OrderServed() {
        // start new recipe
        if (_currentOrderIdx >= _customerOrders.Count - 1) {
            print("finished game");
            //show scores
            rdScoreSystem.Seele.ShowScore();
            return;
        }
        NewRecipe();

    }

    void OnStoryTellingDone() {
        if (_currentOrderIdx >= _customerOrders.Count - 1) {
            print("finished game");
            //show scores
            // rdScoreSystem.Seele.ShowScore();
            return;
        }
        _currentOrderIdx++;
        _CrowdSound.volume = CrowdSoundClip.GetVolume(_currentOrderIdx);
        //NewRecipe();
    }

    //event to call all to replenish/change menu
    public void NewRecipe() {
        if (JustStarted) {
            _currentOrderIdx = 0;
            JustStarted = false;
        } else {
            _currentOrderIdx++;

        }
        OnNewRecipe?.Invoke();
        StoryData[] story = _customerOrders[_currentOrderIdx].StoryData;
        rdUIManager.ShowStoryText(story);
    }

    public static void UpdateInstruction(FoodItemTag foodInstruction) {
        Seele.OnHoldInstructionUpdate?.Invoke(foodInstruction);
    }
}
[Serializable]
public struct AudioAmbienceDynamic {
    public AudioClip Clip;
    [Range(0, 1)]
    public float[] FaderMarks;
    public float GetVolume(int key) {
        return FaderMarks[key];
    }
}