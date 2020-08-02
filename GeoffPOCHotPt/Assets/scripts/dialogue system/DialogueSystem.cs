using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : SimpleSingleton<DialogueSystem> {

    public Canvas MainCanvas;
    public string Path;
    public GameObject DialoguePrefab;

    Dictionary<ConversationType, Conversation> _conversations = new Dictionary<ConversationType, Conversation>();
    int _currentIndex;

    //co-routines stuff
    private Coroutine _typing;
    [SerializeField] WaitForSeconds _typingTime = new WaitForSeconds(0.02f);

    protected override void Awake() {
        base.Awake();

        Conversation[] conversations = Resources.LoadAll<Conversation>(Path);

        foreach (Conversation convo in conversations) {
            _conversations[convo.Type] = convo;
            Debug.Log(convo.Type);
        }
    }

    //test
    private void Start() {
        StartConversation(ConversationType.GameIntro);
    }

    Conversation GetConvo(ConversationType type) {
        Conversation c;
        if (_conversations.TryGetValue(type, out c)) {
            return c;
        } else
            throw new System.Exception("Invialid dialogue " + type);
    }

    public void StartConversation(ConversationType type) {
        Conversation currentConvo = GetConvo(type);
        GameObject panel = Instantiate(DialoguePrefab, MainCanvas.transform);
        UIConvoPanel uiPanel = panel.GetComponent<UIConvoPanel>();

        Text buttonUI = uiPanel.NextButtonUI.GetComponentInChildren<Text>();

        _currentIndex = 0;
        uiPanel.SpeakerNameUI.text = "";
        uiPanel.DialogueLineUI.text = "";
        buttonUI.text = "NEXT";
        uiPanel.NextButtonUI.onClick.AddListener(() => ReadNext(uiPanel, currentConvo, buttonUI));

        ReadNext(uiPanel, currentConvo, buttonUI);
    }
    
    void ReadNext(UIConvoPanel uiPanel, Conversation convo, Text buttonUI) {
        if (_currentIndex > convo.GetLength()) {
            //destroy gameobject or animation
            Destroy(uiPanel.gameObject);
            Debug.Log("Convo over");
            return;
        }

        uiPanel.SpeakerNameUI.text = convo.GetLineByIndex(_currentIndex).Speaker.Name;

        //to stop typing bug
        string dialogue = convo.GetLineByIndex(_currentIndex).Dialogue;
        if (_typing == null) {
            _typing = StartCoroutine(TypeText(dialogue, uiPanel.DialogueLineUI));
        } else {
            StopCoroutine(_typing);
            _typing = null;
            _typing = StartCoroutine(TypeText(dialogue, uiPanel.DialogueLineUI));
        }

        uiPanel.SpeakerImage.sprite = convo.GetLineByIndex(_currentIndex).Speaker.Icon;
        _currentIndex++;

        if (_currentIndex > convo.GetLength()) {
            buttonUI.text = "End Convo";
        }
    }

    IEnumerator TypeText(string text, Text dialogueUI) {
        dialogueUI.text = "";
        bool complete = false;
        int typeIndex = 0;
        //NavButton.gameObject.SetActive(false);

        while (!complete) {
            dialogueUI.text += text[typeIndex];
            typeIndex++;
            yield return _typingTime;

            if (typeIndex == text.Length) {
                //NavButton.gameObject.SetActive(true);
                complete = true;
            }
        }
        _typing = null;
    }
}