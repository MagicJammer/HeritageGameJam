using System;
using UnityEngine;
public class PocCustomerDialogueRating : MonoBehaviour
{
    public int Stars = 5;
    public float TimeLeft;
    public DialogueData[] Script;
    public int ScriptKey;
    void Start()
    {
        ScriptKey = 0;
        TimeLeft = 25;
        Stars = 5;
    }
    bool AdvanceDialogue()
    {
        if (ScriptKey >= Script.Length)
            return false;
        DialogueData l = Script[ScriptKey];
        Debug.Log(l.Character + ": " + l.Lines);
        ScriptKey++;
        return true;
    }
    void AddTime()
    {
        TimeLeft += 5;
        if (TimeLeft > 30)
            TimeLeft = 30;
        int s = Mathf.RoundToInt(TimeLeft/5);
        if (s > 5)
            s = 5;
        Stars = s;
    }
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        int s = Mathf.RoundToInt(TimeLeft/5);
        Stars = s;
        if (TimeLeft <= 0)
            Debug.Log("Customer lost");
        if (Input.GetButtonDown("Fire1"))
            if (AdvanceDialogue())
                AddTime();
        if (Input.GetButtonDown("Fire2"))
            Debug.Log("customer has given you " + Stars + "/5 Stars");
    }
}
public enum Actor
{
    Player,
    Customer,
}
[Serializable]
public struct DialogueData
{
    public Actor Character;
    public string Lines;
    DialogueData(Actor character, string lines)
    {
        Character = character;
        Lines = lines;
    }
}