using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    PlayerEntity _entity;
    Animator _anim;
    AudioSource _source;
    [SerializeField] AudioClip[] _clip;
    [SerializeField] PlayerState _currentState = PlayerState.Idle;


    public void Start()
    {
        _entity = GetComponent<PlayerEntity>();
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        float hPress = Input.GetAxisRaw("Horizontal");
        if (hPress != 0)
        {
            PlayerStateChange(PlayerState.Walking);
            
            bool isWall = _entity.WallCheck();
            if (!isWall)
                _entity.Move(hPress);
            if (hPress * _entity.facing < 0)
                _entity.Flip();


        }
        else if (_entity.GroundCheck())
            PlayerStateChange(PlayerState.Idle);
        else
            PlayerStateChange(PlayerState.OnAir);

        if (Input.GetButtonDown("Jump"))
        {
            
            bool isGround = _entity.GroundCheck();
            if (isGround)
            {
                _entity.Jump();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _entity.HoldIngredient();
            _entity.ActivateWorkStation();
        }
    }


    void PlayerStateChange(PlayerState state)
    {
        if (_currentState != state)
        {
            _currentState = state;
            switch (state)
            {
                case PlayerState.Walking:
                    _currentState = PlayerState.Walking;
                    _anim.SetBool("ToRun", true);
                    _source.clip = _clip[0];
                    _source.loop = true;
                    _source.Play();
                    break;
                case PlayerState.OnAir:
                    _currentState = PlayerState.OnAir;
                    _anim.SetBool("ToRun", false);
                    _source.clip = _clip[1];
                    _source.loop = false;
                    _source.Play();
                    break;
                case PlayerState.Idle:
                    _currentState = PlayerState.Idle;
                    _anim.SetBool("ToRun", false);
                    _source.clip = null;
                    break;

            }
        }
    }
}
