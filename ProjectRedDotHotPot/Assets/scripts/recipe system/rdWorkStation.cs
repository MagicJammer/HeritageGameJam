using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rdWorkStation : rdStation {
    public WorkstationTag Tag;
    public StationStatus Status;
    rdEntity User;
    public RecipeInstruction CurrentInstruction;
    public List<RecipeInstruction> RecipeMenu = new List<RecipeInstruction>();
    public List<FoodItemTag> CurrentHoldItems = new List<FoodItemTag>();
    public float Timer;
    //public bool Done;
    AudioSource LoopSound;
    public AudioOneShotData StartSound;
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
                            rdUIManager.UpdateStationPopups(this.gameObject);
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
                //OnHoldInstructionUpdate();
                rdUIManager.UpdateOnHandItem(user.ItemOnHand, user);
                rdUIManager.UpdateStationPopups(this.gameObject);
                rdRecipeManager.UpdateInstruction(user.ItemOnHand);
                break;
            default:
                return false;
        }
        return false;
    }

    public override void ResetRecipe() {

    }

    private void OnDestroy() {
        if (rdRecipeManager.Seele != null) {
            rdRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
            rdRecipeManager.Seele.OnHoldInstructionUpdate -= OnHoldInstructionUpdate;
        }
    }

    private void Start() {
        rdRecipeManager.Seele.OnNewRecipe += OnNewRecipe;
        rdRecipeManager.Seele.OnHoldInstructionUpdate += OnHoldInstructionUpdate;
        OnNewRecipe();
        LoopSound = GetComponent<AudioSource>();
    }

    public void OnNewRecipe() {
        RecipeMenu.Clear();
        foreach (RecipeInstruction ins in rdRecipeManager.Seele._currentRecipe.Instructions) {
            if (Tag == ins.Workstation) {
                RecipeMenu.Add(ins);
            }
        }
        Status = StationStatus.Ready;
        //UpdatePopups();
    }

    void OnHoldInstructionUpdate(FoodItemTag food) {
        List<FoodItemTag> fds = new List<FoodItemTag>();
        foreach (RecipeInstruction item in RecipeMenu) {
            fds.Add(item.Result);
        }
        
        //to be updated later XD
        foreach (RecipeInstruction rcp in RecipeMenu) {
            foreach (FoodItemTag correctFood in rcp.Ingredients) {
                if(food == correctFood) {
                    rdUIManager.UpdateStationPopups(this.gameObject, correctFood);
                    break;
                }
            }
        }
    }

    void StartTask(rdEntity user) {
        Timer += CurrentInstruction.ProcessTime;
        Status = StationStatus.Cooking;
        User = user;
        if(LoopSound.clip!=null)
        LoopSound.Play();
        StartSound.PlayAtPoint(transform.position);
        //play start sound and loop sound
    }

    void TaskDone() {
        User.SendMessageToBrain((int)PlayerCommand.WorkDone);
        Debug.Log(CurrentInstruction.Result + " Done");
        LoopSound.Stop();
        Status = StationStatus.Collect;
        if(CurrentInstruction.Type==TaskType.Active)
        {
            User.CollectItem(CurrentInstruction.Result);
            RecipeMenu.Remove(CurrentInstruction);
            CurrentHoldItems.Clear();
            //OnHoldInstructionUpdate();
            rdUIManager.UpdateOnHandItem(User.ItemOnHand, User);
            rdUIManager.UpdateStationPopups(this.gameObject);
            rdRecipeManager.UpdateInstruction(User.ItemOnHand);
            Status = StationStatus.Ready;
        }
        else
        {
        rdUIManager.UpdateStationPopups(this.gameObject, CurrentInstruction.Result);

        }
        User = null;
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