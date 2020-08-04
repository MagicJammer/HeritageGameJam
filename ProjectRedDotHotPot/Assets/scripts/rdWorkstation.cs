using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rdWorkstation : rdStation
{
    public WorkstationTag Tag;
    public StationStatus Status;
    public float Timer;
    rdEntity User;
    RecipeInstruction CurrentInstruction;
    List<FoodItemTag> ItemsOnHold = new List<FoodItemTag>();
    List<RecipeInstruction> Instructions = new List<RecipeInstruction>();
    //bool Cooking = false;
    private void Awake()
    {
        OrderCor.Sub(0, GameStart);
    }
    private void GameStart(object sender)
    {
        rdRecipeManager.Seele.KitchenStations[Tag] = this;
    }
    public override void ResetRecipe()
    {
        Status = StationStatus.Inactive;
        ItemsOnHold.Clear();
        Instructions.Clear();
        Timer = 0;
    }
    public void SetRecipe(RecipeInstruction instruction)
    {
        Instructions.Add(instruction);
        Status = StationStatus.Ready;
    }
    public override bool Interact(FoodItemTag item, rdEntity user)
    {
        switch (Status)
        {
            case StationStatus.Inactive:
            case StationStatus.Cooking:
                return false;
            case StationStatus.Ready:
                if (AddItem(item))
                {
                    user.DropOffItem();
                    StartTask(user);
                    return CurrentInstruction.Type == TaskType.Active;// if true player goes into work mode, might need an enum for process type
                }
                else
                    return false;
            case StationStatus.Collect:
                if (user.CollectItem(CurrentInstruction.Result))
                    Status = StationStatus.Ready;
                return false;
        }
        return false;
        //if (Status == WorkstationStatus.Inactive)
        //    return false;
        //return CurrentInstruction.Type==TaskType.Active;// if true player goes into work mode, might need an enum for process type
    }
    public bool AddItem(FoodItemTag item)
    {
        foreach (RecipeInstruction instruction in Instructions)
            foreach (FoodItemTag food in instruction.Ingredients)
                if (food == item)
                    CurrentInstruction = instruction;
                else return false;
        ItemsOnHold.Add(item);
        foreach(FoodItemTag i in CurrentInstruction.Ingredients)
            if(!ItemsOnHold.Contains(i))
              return false;
        return true;
    }
    void StartTask(rdEntity user)
    {
        Timer = CurrentInstruction.ProcessTime;
        Status = StationStatus.Cooking;
        User = user;
    }
    void TaskDone()
    {
        User.SendMessageToBrain((int)PlayerCommand.WorkDone);
        Debug.Log(CurrentInstruction.Result + " Done");
        User = null;
        Status = StationStatus.Collect;
    }
    void Update()
    {
        if (Status == StationStatus.Cooking)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
                TaskDone();
        }
    }
}
