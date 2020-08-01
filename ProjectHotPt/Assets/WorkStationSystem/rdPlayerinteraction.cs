using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdPlayerinteraction : MonoBehaviour
{
    public rdWorkstation SelectedWorkstation;
    public IngredientType IngredientInHand;
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            SelectedWorkstation.ActivateWorkstation(IngredientInHand);
        }
    }
}