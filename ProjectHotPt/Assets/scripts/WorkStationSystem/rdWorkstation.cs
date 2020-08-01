using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdWorkstation : MonoBehaviour
{
    public WorkstationType Workstation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject cgo = collision.gameObject;
        if (cgo.layer == LayerMask.NameToLayer("Player"))
        {
            rdPlayerinteraction player = cgo.GetComponent<rdPlayerinteraction>();
            player.SelectedWorkstation = this;
            Debug.Log("registered");
        }
        else
        {
            Debug.Log("not registered");
        }
    }
    public void ActivateWorkstation(rdIngredientType ingredient)
    {
        Debug.Log("use " + ingredient + " with " + Workstation);
    }
}
public enum WorkstationType
{
    none,
    Stove, 
    FryingPan, 
    SoupBoiler,
    Oven,
    ChoppingBoard,
}
public enum rdIngredientType
{
    none,
    Cabbage,
    Onions,
    Garlic,
    Corn,
    Radish,
    Mushrooms,
}