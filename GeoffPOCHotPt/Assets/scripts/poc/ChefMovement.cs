using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefMovement : MonoBehaviour {
    [Header("Components")]
    public Rigidbody2D Rgdby2D;

    [Header("Horizontal Movement")]
    public float MoveSpeed = 4f;
    [SerializeField] Vector2 _inputDir;
    static Vector3 _velocity;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float JumpDelay = 0.25f;
    float _jumpTimer;

    [Header("Physics")]
    public float Gravity = 1f;
    public float FallMultiplier = 5f;
    public float LinearDrag = 4f;

    [Header("Collision")]
    public bool OnGround = false;
    public float GroundLength = 0.6f;
    public Vector3 ColliderOffset;
    public LayerMask GroundLayer;

    // Start is called before the first frame update
    void Start() {
        Rgdby2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        OnGround = Physics2D.Raycast(transform.position + ColliderOffset, Vector2.down, GroundLength, GroundLayer)
                 || Physics2D.Raycast(transform.position - ColliderOffset, Vector2.down, GroundLength, GroundLayer);

        if (Input.GetKeyDown(KeyCode.Space)) {
            _jumpTimer = Time.time + JumpDelay;
        }
        _inputDir.x = Input.GetAxis("Horizontal");

        _velocity = Rgdby2D.velocity;
    }

    private void FixedUpdate() {
        MoveCharacter(_inputDir.x);

        if (_jumpTimer > Time.time && OnGround) {
            Jump();
        }

        ModifyPhysics();
    }

    void Jump() {
        Rgdby2D.velocity = new Vector2(Rgdby2D.velocity.x, 0);
        Rgdby2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        _jumpTimer = 0;
    }

    void MoveCharacter(float horizontal) {
        Rgdby2D.velocity = new Vector2(horizontal * MoveSpeed, Rgdby2D.velocity.y);
    }

    void ModifyPhysics() {
        if (OnGround) {
            if (Mathf.Abs(_inputDir.x) < 0.4f) {
                Rgdby2D.drag = LinearDrag;
            } else {
                Rgdby2D.drag = 0f;
            }
            Rgdby2D.gravityScale = 0;
        } else {
            Rgdby2D.gravityScale = Gravity;
            Rgdby2D.drag = LinearDrag * 0.15f;
            float rbVelocityY = Rgdby2D.velocity.y;
            if (rbVelocityY < 0) {
                Rgdby2D.gravityScale = Gravity * FallMultiplier;
            } else if (rbVelocityY > 0 && !Input.GetButton("Jump")) {
                Rgdby2D.gravityScale = Gravity * (FallMultiplier / 2);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + ColliderOffset, transform.position + ColliderOffset + Vector3.down * GroundLength);
        Gizmos.DrawLine(transform.position - ColliderOffset, transform.position - ColliderOffset + Vector3.down * GroundLength);
    }
}