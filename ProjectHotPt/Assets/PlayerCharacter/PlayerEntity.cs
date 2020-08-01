using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public float JumpForce = 1;
    public float MoveSpeed = 1;

    public float GroundCheckDistance = 0.1f;

    public LayerMask PlatformMask = 1 << 8;

    [HideInInspector]
    public Rigidbody2D _RB2D;

    [HideInInspector]
    public SpriteRenderer _SP;
    BoxCollider2D col;

    [HideInInspector]
    public float facing = 1;

    // Start is called before the first frame update
    void Awake()
    {
        _RB2D = GetComponent<Rigidbody2D>();
        _SP = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

    }

    public bool GroundCheck()
    {
        float footOffset = col.size.x / 2;
        float heightOffset = col.size.y / 2;
        Vector2 footPos = transform.position;
        Vector2 footLeft = footPos;
        footLeft.x -= footOffset;
        Vector2 footRight = footPos;
        footRight.x += footOffset;

        bool grounded = Physics2D.Raycast(footLeft, Vector2.down, GroundCheckDistance, PlatformMask) || Physics2D.Raycast(footRight, Vector2.down, heightOffset + GroundCheckDistance, PlatformMask) || Physics2D.Raycast(footPos, Vector2.down, GroundCheckDistance, PlatformMask);
        return grounded;
    }

    public bool WallCheck()
    {
        float offset = col.size.x / 2; ;
        bool facingWall = Physics2D.Raycast(transform.position, Vector2.right * facing, offset, PlatformMask);
        return facingWall;
    }

    public void Move(float hPress)
    {
        Vector2 currentPos = transform.position;
        currentPos.x += hPress * MoveSpeed * Time.deltaTime;
        transform.position = currentPos;
    }

    public void Jump()
    {
        _RB2D.AddForce(Vector2.up * JumpForce);
    }

    public void Flip()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        facing *= -1;
        transform.localScale = temp;
    }
}
