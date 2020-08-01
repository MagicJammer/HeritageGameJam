using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    PlayerEntity _entity;
    Animator _anim;

    public void Start()
    {
        _entity = GetComponent<PlayerEntity>();
        _anim = GetComponent<Animator>();
    }

    public void Update()
    {
        float hPress = Input.GetAxis("Horizontal");
        if (hPress != 0)
        {
            bool isWall = _entity.WallCheck();
            if (!isWall)
                _entity.Move(hPress);
            if (hPress * _entity.facing < 0)
                _entity.Flip();

            _anim.SetBool("ToRun", true);
                            
        }
        else
            _anim.SetBool("ToRun", false);

        if (Input.GetButtonDown("Jump"))
        {
            bool isGround = _entity.GroundCheck();
            if (isGround)
                _entity.Jump();
        }
    }
}
