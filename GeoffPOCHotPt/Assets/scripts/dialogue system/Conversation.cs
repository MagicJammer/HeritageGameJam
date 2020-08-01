using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dyep/Conversation")]
public class Conversation : ScriptableObject {

    public ConversationType Type;

    [SerializeField] 
    DialogueLine[] _dialogueLines = new DialogueLine[0];

    public DialogueLine GetLineByIndex(int index) {
        return _dialogueLines[index];
    }

    public int GetLength() {
        return _dialogueLines.Length - 1; //in order to get the correct index
    }
}

[System.Serializable]
public class DialogueLine {
    public Speaker Speaker;
    [TextArea]
    public string Dialogue;

    public float Duration; //auto duration i guess
}

public enum ConversationType {
    GameIntro,
    GirlIntro,
    BoyIntro
}