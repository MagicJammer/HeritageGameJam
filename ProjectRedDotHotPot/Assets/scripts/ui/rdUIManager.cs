using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rdUIManager : Singleton<rdUIManager>
{
    public GameObject PopupPrefab;
    public float PopupOffsetY = 1.75f;

    public string FoodPath = "ui_foodbags";
    Dictionary<FoodItemTag, rdUIFoodBags> _foodBags = new Dictionary<FoodItemTag, rdUIFoodBags>();

    public event Action<string> OnDestroyPopup;
 
    protected override void Awake() {
        base.Awake();

        rdUIFoodBags[] fd= Resources.LoadAll<rdUIFoodBags>(FoodPath);
        foreach (var f in fd) {
            _foodBags[f.FoodType] = f;
        }
    }

    public static void UpdateStationPopups(GameObject station, List<FoodItemTag> requirements) {
        FoodItemTag[] reqArray = requirements.ToArray();
        UpdateStationPopups(station, reqArray);
    }

    public static void UpdateStationPopups(GameObject station, FoodItemTag[] requirements) {
        Seele.DestroyPopup(station.name);
        GameObject popupGO = Instantiate(Seele.PopupPrefab);
        popupGO.transform.SetParent(station.transform);
        rdUIWorldSpaceCanvas popupCanvas = popupGO.GetComponent<rdUIWorldSpaceCanvas>();
        popupCanvas.ID = station.name;
        RectTransform popupRect = popupGO.GetComponent<RectTransform>();
        popupRect.localPosition = new Vector3(0, Seele.PopupOffsetY, 0);
        Transform parent = popupGO.transform.GetChild(0);
        foreach (FoodItemTag item in requirements) {
            if (Seele._foodBags.TryGetValue(item, out rdUIFoodBags foodbag)) {
                GameObject fdGO = Instantiate(foodbag.Prefab);
                fdGO.transform.SetParent(parent);
                fdGO.transform.localScale = Vector3.one;
            }
        }
    }

    public void DestroyPopup(string id) {
        OnDestroyPopup?.Invoke(id);
    }
}
