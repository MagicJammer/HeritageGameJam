using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerScript : MonoBehaviour
{
    float BoilingTime;
    float ElapsedTime = 0;
    CookingRecipe _recipe;

    public bool toBoil;
    // Start is called before the first frame update
    void Start()
    {
        _recipe = GetComponent<WorkStationScript>().CurrentCookingRecipe;
        BoilingTime = _recipe.CookingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (toBoil)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= BoilingTime)
            {
                toBoil = false;
                Debug.Log("Cooking Done");
            }
        }

    }

    //void Boil()
    //{
    //    if (_recipe.Action == CookAction.Boil)
    //    {
    //        ElapsedTime += Time.deltaTime;
    //        if (ElapsedTime >= BoilingTime)
    //        {
    //            toBoil = false;
    //            Debug.Log("Cooking Done");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Wrong action");
    //    }
    //}

    
}
