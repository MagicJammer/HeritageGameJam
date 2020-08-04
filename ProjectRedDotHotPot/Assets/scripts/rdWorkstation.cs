using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rdWorkstation : rdStation
{
    public WorkstationStatus Status;
    public float Timer;
    rdEntity User;
    RecipeInstruction CurrentInstruction;
    List<FoodItemTag> ItemsOnHold = new List<FoodItemTag>();
    List<RecipeInstruction> Instructions = new List<RecipeInstruction>();
    bool Cooking = false;
    public override void ResetRecipe()
    {
        Status = WorkstationStatus.Inactive;
        ItemsOnHold.Clear();
        Instructions.Clear();
        Timer = 0;
    }
    public void SetRecipe(RecipeInstruction instruction)
    {
        Instructions.Add(instruction);
        Status = WorkstationStatus.Ready;
    }
    public override bool Interact(FoodItemTag item, rdEntity user)
    {
        switch (Status)
        {
            case WorkstationStatus.Inactive:
            case WorkstationStatus.Cooking:
                return false;
            case WorkstationStatus.Ready:
                if (AddItem(item))
                {
                    user.DropOffItem();
                    return CurrentInstruction.Type == TaskType.Active;// if true player goes into work mode, might need an enum for process type
                }
                else
                    return false;
            case WorkstationStatus.Collect:
                if (user.CollectItem(CurrentInstruction.Result))
                    Status = WorkstationStatus.Ready;
                return false;
        }
        return false;
        //if (Status == WorkstationStatus.Inactive)
        //    return false;
        //return CurrentInstruction.Type==TaskType.Active;// if true player goes into work mode, might need an enum for process type
    }
    public bool AddItem(FoodItemTag item)
    {
        foreach(RecipeInstruction instruction in Instructions)
            foreach(FoodItemTag food in instruction.Ingredients)
                if (food == item)
                    CurrentInstruction = instruction;
        ItemsOnHold.Add(item);
        foreach(FoodItemTag i in CurrentInstruction.Ingredients)
            if(!ItemsOnHold.Contains(i))
              return false;
        return true;
    }
    void StartTask(rdEntity user)
    {
        Timer = CurrentInstruction.ProcessTime;
        Status = WorkstationStatus.Cooking;
        User = user;
    }
    void TaskDone()
    {
        User.SendMessageToBrain((int)PlayerCommand.WorkDone);
        Debug.Log(CurrentInstruction.Result + " Done");
        User = null;
        Status = WorkstationStatus.Collect;
    }
    void Update()
    {
        if (Cooking)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
                TaskDone();
        }
    }
}
