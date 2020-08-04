using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ingedrient", menuName = "MagicJammer/food bag", order = 2)]
public class rdUIFoodBags : ScriptableObject
{
    public FoodItemTag FoodType;
    public Sprite Sprite;
    public GameObject Prefab;
}
