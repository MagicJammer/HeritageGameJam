using System;
using System.Collections.Generic;
using UnityEngine;
public class rdPlayerController : MonoBehaviour
{
    rdEntity _entity;
    void Start()
    {
        _entity = GetComponent<rdEntity>();
    }
    void Update()
    {
        float hPress = Input.GetAxis("Horizontal");
        //if (hPress != 0)
                _entity.SendMessageToBrain((int)PlayerCommand.Walk,hPress);

        //Toan's code start
        if (hPress * transform.localScale.x >  0)
        {
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
        //Toan's code end
        if (Input.GetButtonDown("Jump"))
            _entity.SendMessageToBrain((int)PlayerCommand.Jump);
        if (Input.GetButtonDown("Fire1"))
            _entity.SendMessageToBrain((int)PlayerCommand.Interact);
    }
}
public enum PlayerCommand
{
    Walk,
    Jump,
    Interact,
}
[Serializable]
public struct AudioOneShotData
{
    public AudioClip[] Clips;
    [Range(0, 1)]
    public float Volume;
    int _CurrentClip;
    public void PlayAtPoint(Vector3 position)
    {
        if (Clips[_CurrentClip] != null)
        {
        AudioSource.PlayClipAtPoint(Clips[_CurrentClip], position, Volume);
        _CurrentClip++;
        if (_CurrentClip >= Clips.Length)
            _CurrentClip = 0;
        }
    }
} 