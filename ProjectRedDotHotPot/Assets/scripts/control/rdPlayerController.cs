using System.Collections;
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