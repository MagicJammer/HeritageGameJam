using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rdUIManager : Singleton<rdUIManager>
{
    [Header("Main Canvas")]
    public GameObject MainCanvasPrefab;
    public Canvas MainCanvas { private set; get; }
    [Header("Popup Canvas")]
    public GameObject PopupPrefab;
    public float PopupOffsetY = 1.75f;
    [Header("Story/Chat Canvas")]
    public GameObject ChatPrefab;
    GameObject _currentChatGO;
    Text _currentText;
    int _currentIdx=0;
    StoryData[] _stories;
    //ChatData[] _chats;

    [Header("FoodPath")]
    public string FoodPath = "ui_foodbags";
    Dictionary<FoodItemTag, rdUIFoodBags> _foodBags = new Dictionary<FoodItemTag, rdUIFoodBags>();

    [Header("Offset for the player")]
    public Vector3 Offset;
    [HideInInspector]
    public Transform PlayerEntity;
    [HideInInspector]

    public Transform Customer;

    public event Action<string> OnDestroyPopup;
    public event Action OnStoryTellingDone;
 
    protected override void Awake() {
        base.Awake();

        rdUIFoodBags[] fd= Resources.LoadAll<rdUIFoodBags>(FoodPath);
        foreach (var f in fd) {
            _foodBags[f.FoodType] = f;
        }

        if (MainCanvas == null) {
            MainCanvas = Instantiate(MainCanvasPrefab).GetComponent<Canvas>();
        }
    }

    public static void UpdateStationPopups(GameObject station, FoodItemTag req) {
        FoodItemTag[] reqToArray = new FoodItemTag[] { req};
        UpdateStationPopups(station, reqToArray);
    }

    public static void UpdateStationPopups(GameObject station, List<FoodItemTag> requirements) {
        FoodItemTag[] reqArray = requirements.ToArray();
        UpdateStationPopups(station, reqArray);
    }

    public static void UpdateStationPopups(GameObject station, FoodItemTag[] requirements = null) {
        Seele.DestroyPopup(station.name);
        if (requirements == null) {

            return;
        }
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
                Image im = fdGO.GetComponent<Image>();
                im.sprite = foodbag.Sprite;
                fdGO.transform.SetParent(parent);
                fdGO.transform.localScale = Vector3.one;
            }
        }
    }

    public static void UpdateOnHandItem(FoodItemTag type, rdEntity entity) {
        if (Seele._foodBags.TryGetValue(type, out rdUIFoodBags fdbag)) {
            entity.ItemOnHandSprite.sprite = fdbag.Sprite;
        } else
            entity.ItemOnHandSprite.sprite = null;
    }

    /// <summary>
    /// events section theres a better way but yeah im lazy xD
    /// </summary>
    /// <param name="id"></param>
    public void DestroyPopup(string id) {
        OnDestroyPopup?.Invoke(id);
    }

    public static void ShowStoryText(StoryData[] stories) 
    {
        if (Seele._currentChatGO != null)
            Destroy(Seele._currentChatGO);

        Seele._currentIdx = 0;
        Seele._stories = stories;
        Seele._currentChatGO = Instantiate(Seele.ChatPrefab);
        Seele._currentText = Seele._currentChatGO.GetComponentInChildren<Text>();
        Seele._currentText.text = "";
        Seele.ReadNext();
    }

    void ReadNext() 
    {
        float delay = 5f;
        if (_currentIdx > _stories.Length - 1) 
        {
            CancelInvoke();
            print("finised story");
           // Invoke("StoryDone", delay);
            Destroy(_currentChatGO, delay);
            return;
        }
        delay = _stories[_currentIdx].TextDelay;
        if (_stories[_currentIdx].Speaker == Speaker.Customer) {
            _currentChatGO.transform.SetParent(null, false);
            _currentChatGO.transform.position = Customer.position + Offset;
        } else {
            _currentChatGO.transform.position = PlayerEntity.position + Offset;
            _currentChatGO.transform.SetParent(PlayerEntity);
        }
        _currentText.text = _stories[_currentIdx].StoryLine;
        _currentIdx++;
        Invoke("ReadNext", delay);
    }

    void StoryDone() 
    {
        OnStoryTellingDone?.Invoke();
    }

    public static void ShowRandomChatText(ChatData[] chats) {
        if (Seele._currentChatGO != null)
            Destroy(Seele._currentChatGO);

        Seele._currentIdx = 0;
        Seele._currentChatGO = Instantiate(Seele.ChatPrefab);
        Seele._currentText = Seele._currentChatGO.GetComponentInChildren<Text>();
        Seele._currentText.text = "";

        int randChatIdx = UnityEngine.Random.Range(0, chats.Length);
        Seele._currentText.text = chats[randChatIdx].ChatLine;
        Destroy(Seele._currentChatGO, chats[randChatIdx].TextDelay);
    }
}