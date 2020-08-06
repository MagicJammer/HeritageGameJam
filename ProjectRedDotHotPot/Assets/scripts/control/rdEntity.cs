using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlayerState 
{ 
    Unassigned,
    Move, 
    Work, 
    //Chat, 
}
public class rdEntity : FiniteStateMachine<PlayerState>
{
    public override PlayerState UnassignedType => PlayerState.Unassigned;
    public FoodItemTag ItemOnHand;
    public rdStation SelectedStation;
    public float MoveSpeed = 10;
    public float JumpForce = 450;
    public float GroundCheckDistance = 0.1f;
    public LayerMask PlatformMask = 1 << 8;
    public AudioClip JumpSound;
    [Range(0,1)]
    public float Volume = 1;
    //[HideInInspector]
    public float facing = 1;
    [HideInInspector]
    public Rigidbody2D _RB2D;
    [HideInInspector]
    public BoxCollider2D col;
    public SpriteRenderer ItemOnHandSprite;
    
    void Start()
    {
        _RB2D = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        RegisterState(new MoveState(this));
        RegisterState(new WorkState(this));
        ChangeState(PlayerState.Move);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMachine();
    }
    public void DropOffItem()
    {
        ItemOnHand = FoodItemTag.None;
    }
    public bool CollectItem(FoodItemTag item)
    {
        if (ItemOnHand == FoodItemTag.None)
        {
            ItemOnHand = item;
            return true;
        } else {
            return false;
        }

    }
}
public class MoveState : rdEntity.SI_State<rdEntity>
{
    public override PlayerState StateType => PlayerState.Move;
    public MoveState(rdEntity brain) : base(brain) { }
    public override void OnReceiveMessage(int msgtype, object[] args)
    {
        PlayerCommand c = (PlayerCommand)msgtype;
        switch (c)
        {
            case PlayerCommand.Walk:
                float moveX = (float)args[0];
                if (moveX > 0)
                    user.facing = 1;
                else if(moveX<0)
                    user.facing = -1;
                if (WallCheck())
                    return;
                //Vector2 currentPos = user.transform.position;
                //currentPos.x += moveX * user.MoveSpeed * Time.deltaTime;
                //user.transform.position = currentPos;
                Rigidbody2D r = user._RB2D;
                r.velocity = new Vector2(moveX * user.MoveSpeed, r.velocity.y);
                break;
            case PlayerCommand.Jump:
                if (GroundCheck())
                {
                user._RB2D.AddForce(Vector2.up * user.JumpForce);
                    AudioSource.PlayClipAtPoint(user.JumpSound, user.transform.position, user.Volume);
                }
                break;
            case PlayerCommand.Interact:
                if (user.SelectedStation != null)
                if (user.SelectedStation.Interact(user.ItemOnHand,user))
                    user.ChangeState(PlayerState.Work);
                break;
        }
    }
    public bool WallCheck()
    {
        float offset = user.col.size.x / 2; ;
        bool facingWall = Physics2D.Raycast(user.transform.position, Vector2.right * user.facing, offset, user.PlatformMask);
        return facingWall;
    }
    public bool GroundCheck()
    {
        BoxCollider2D c = user.col;
        float footOffset = c.size.x / 2;
        float heightOffset = c.size.y / 2;
        Vector2 footPos = user.transform.position;
        Vector2 footLeft = footPos;
        footLeft.x -= footOffset;
        Vector2 footRight = footPos;
        footRight.x += footOffset;
        bool leftCheck = Physics2D.Raycast(footLeft, Vector2.down, heightOffset + user.GroundCheckDistance, user.PlatformMask);
        bool rightCheck= Physics2D.Raycast(footRight, Vector2.down, heightOffset + user.GroundCheckDistance, user.PlatformMask);
        return leftCheck||rightCheck;
    }
    public override void OnStateEnter(PlayerState prevStateType, object[] args)
    { 
        user =(rdEntity)Machine;

    }

    public override void OnStateExit(PlayerState newStateType, object[] arg)
    { 
    }

    public override void OnStateUpdate()
    { 
    }
}
public class WorkState : rdEntity.SI_State<rdEntity>
{
    public override PlayerState StateType => PlayerState.Work;
    public WorkState(rdEntity brain) : base(brain) { }
    public override void OnReceiveMessage(int msgtype, object[] args)
    {
        PlayerCommand c = (PlayerCommand)msgtype;
        if (c == PlayerCommand.WorkDone)
            user.ChangeState(PlayerState.Move);
    }

    public override void OnStateEnter(PlayerState prevStateType, object[] args)
    {
        user = (rdEntity)Machine;
        Rigidbody2D r = user._RB2D;
        r.velocity = Vector2.zero;
    }

    public override void OnStateExit(PlayerState newStateType, object[] arg)
    {
    }

    public override void OnStateUpdate()
    {
    }
}