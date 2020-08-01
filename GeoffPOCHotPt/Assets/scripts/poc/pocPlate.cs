using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocPlate : MonoBehaviour, IInteractable {
    public bool HoldPickup;
    public bool PickedUp => HoldPickup;

    public InteractableType InteractableType;
    public InteractableType InteractType => InteractableType;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void GetCookedItem(GameObject cookedItem) {
        //TODO we can just check via ingredient type later, and gfx only not the ingredient itself
        GameObject fx = Instantiate(cookedItem, this.transform.position + Vector3.up, Quaternion.identity);
        fx.transform.SetParent(this.transform);
    }

    //TODO store the ingredients
    public void StoreIngredients() {

    }

    public void PickUpItem(GameObject child, GameObject parent = null) {
        HoldPickup = !HoldPickup;
        if (parent != null) {
            child.transform.SetParent(parent.transform);
            return;
        }
        child.transform.SetParent(null);
    }
}