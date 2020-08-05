using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newWorkStation : rdStation {
    public WorkstationTag Tag;
    public StationStatus Status;
    rdEntity User;
    public RecipeInstruction CurrentInstruction;
    public List<RecipeInstruction> RecipeMenu = new List<RecipeInstruction>();
    public List<FoodItemTag> CurrentHoldItems = new List<FoodItemTag>();
    public float Timer;
    public bool Done;

    //interact only true if the player is need to be working
    public override bool Interact(FoodItemTag item, rdEntity user) {
        switch (Status) {
            case StationStatus.Ready:
                foreach (RecipeInstruction rm in RecipeMenu) {
                    foreach (FoodItemTag igts in rm.Ingredients) {
                        if (igts == item) {
                            user.DropOffItem();
                            CurrentInstruction = rm;
                            rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                            //TODO multiple cooks
                            CurrentHoldItems.Add(igts);
                            FoodItemTag[] reqs = CurrentInstruction.Ingredients;
                            bool startCooking = CurrentHoldItems.OrderBy(x => x).SequenceEqual(reqs.OrderBy(x => x));
                            if (startCooking) {
                                StartTask(user);
                            }
                            return CurrentInstruction.Type == TaskType.Active;
                        }
                    }
                }
                break;
            case StationStatus.Collect:
                if (user.CollectItem(CurrentInstruction.Result))
                    Status = StationStatus.Ready;

                RecipeMenu.Remove(CurrentInstruction);
                CurrentHoldItems.Clear();
                UpdatePopups();
                rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                break;
            default:
                return false;
        }
        return false;
    }

    public override void ResetRecipe() {

    }

    private void OnDestroy() {
        if (newRecipeManager.Seele != null)
            newRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
    }

    private void Start() {
        newRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        OnNewRecipe();
    }

    public void OnNewRecipe() {
        RecipeMenu.Clear();
        foreach (RecipeInstruction ins in newRecipeManager.Seele._currentRecipe.Instructions) {
            if (Tag == ins.Workstation) {
                RecipeMenu.Add(ins);
            }
        }
        Status = StationStatus.Ready;
        UpdatePopups();
    }

    void UpdatePopups() {
        List<FoodItemTag> fds = new List<FoodItemTag>();
        foreach (RecipeInstruction item in RecipeMenu) {
            fds.Add(item.Result);
        }
        rdUIManager.UpdateStationPopups(this.gameObject, fds);
    }

    void StartTask(rdEntity user) {
        Timer += CurrentInstruction.ProcessTime;
        Status = StationStatus.Cooking;
        User = user;
    }

    void TaskDone() {
        User.SendMessageToBrain((int)PlayerCommand.WorkDone);
        Debug.Log(CurrentInstruction.Result + " Done");
        User = null;
        Status = StationStatus.Collect;
    }

    void Update() {
        if (Status == StationStatus.Cooking) {
            Timer -= Time.deltaTime;
            if (Timer < 0) {
                TaskDone();
            }
        }
    }
}