using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocIngredient : MonoBehaviour, IInteractable {
    [Header("Ingredient values")]
    public InteractableType InteractableType;
    public InteractableType InteractType => InteractableType;
    public IngredientType IngredientType;
    public float CookTime;

    public bool HoldPickup;
    public bool PickedUp => HoldPickup;

    public void PickUpItem(GameObject child, GameObject parent = null) {
        HoldPickup = !HoldPickup;
        if (parent != null) {
            child.transform.SetParent(parent.transform);
            return;
        }
        child.transform.SetParent(null);
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}