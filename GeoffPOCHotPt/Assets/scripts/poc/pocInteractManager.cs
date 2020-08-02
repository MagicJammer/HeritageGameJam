using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocInteractManager : SimpleSingleton<pocInteractManager>
{
    public MenuRecipe[] Recipes;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Recipes.Length; i++) {
            print(Recipes[i].Name);
            foreach (var item in Recipes[i].Recipes) {
                print("requires " + item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IInteractable {
    bool PickedUp { get; }
    InteractableType InteractType { get; }
    void PickUpItem(GameObject child, GameObject parent = null);
}

[System.Serializable]
public class MenuRecipe {
    public string Name;
    public GameObject Prefab;
    public IngredientType[] Recipes;
}

public enum IngredientType {
    None,
    Cabbage,
    Chicken,
}

public enum InteractableType {
    None,
    Ingredient,
    Plate,

}
