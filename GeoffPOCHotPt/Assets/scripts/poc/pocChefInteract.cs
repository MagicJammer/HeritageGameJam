using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class pocChefInteract : MonoBehaviour {
    public GameObject _currentPickup;
    public float InteractRange = 1f;
    public Vector3 InteractOffset;
    public LayerMask PickableLayers;
    public LayerMask WorkStationLayer;
    public bool IsOnWorkStation;
    public InteractableType _currentInteractableType;
    public KeyCode InteractKey = KeyCode.E;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 currentPos = this.transform.position;
        if (Input.GetKeyUp(InteractKey) && _currentPickup != null) {
            Collider2D workStationCol = Physics2D.OverlapCircle(currentPos + InteractOffset, InteractRange, WorkStationLayer);
            if (workStationCol != null && workStationCol.TryGetComponent(out pocWorkStation ws) && ws.IsPlayerNearby) {
                switch (_currentInteractableType) {
                    case InteractableType.Ingredient:
                        pocIngredient ingredient = _currentPickup.GetComponent<pocIngredient>();
                        ws.CookingTimeUpdate(ingredient.CookTime, ingredient.IngredientType);
                        //TODO
                        Destroy(_currentPickup);
                        _currentPickup = null;
                        break;
                    case InteractableType.Plate:
                        IngredientType[] currentRecipes = ws.GetCurrentCookedIngredients();
                        foreach (MenuRecipe menuRecipe in pocInteractManager.Instance.Recipes) {
                            IngredientType[] recipeType = menuRecipe.Recipes;
                            if (recipeType.Length != currentRecipes.Length) 
                                continue;                            
                            bool correctMenu = recipeType.OrderBy(x => x).SequenceEqual(currentRecipes.OrderBy(x => x));
                            if (correctMenu) {
                                print("cooked " + menuRecipe.Name);
                                pocPlate plate = _currentPickup.GetComponent<pocPlate>();
                                plate.GetCookedItem(menuRecipe.Prefab);
                                break;
                            } 
                        }
                        break;
                }
                return;
            }
            //no workstation found
            _currentPickup.transform.SetParent(null);
            _currentPickup = null;
            _currentInteractableType = InteractableType.None;
            return;
        }

        if (Input.GetKeyUp(InteractKey) && _currentPickup == null) {
            Collider2D itemCol = Physics2D.OverlapCircle(currentPos + InteractOffset, InteractRange, PickableLayers);
            if (itemCol != null && itemCol.TryGetComponent(out IInteractable i)) {
                i.PickUpItem(itemCol.gameObject, this.gameObject);
                _currentInteractableType = i.InteractType;
                print("pickup item");
                _currentPickup = itemCol.gameObject;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector3 currentPos = this.transform.position;
        Gizmos.DrawWireSphere(currentPos + InteractOffset, InteractRange);
    }
}