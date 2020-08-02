using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity2 : MonoBehaviour
{
    public float JumpForce = 1;
    public float MoveSpeed = 1;

    public float GroundCheckDistance = 0.1f;

    public LayerMask PlatformMask = 1 << 8;

    [HideInInspector]
    public Rigidbody2D _RB2D;

    [HideInInspector]
    public SpriteRenderer _SP;

    [HideInInspector]
    public BoxCollider2D col;

    [HideInInspector]
    public float facing = 1;

    [HideInInspector]
    public PlayerState State;

    Ingredient _onHand;
    GameObject pickUp;

    public WorkStationScript currentWorkStation;

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

    public void HoldIngredient ()
    {

        if (_onHand == null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Interactable"));
            if (Physics2D.BoxCast(transform.position, col.size, 0, Vector2.up, filter, hits, 0.1f) > 0)
            {
                pickUp = hits[0].collider.gameObject;
                if (!pickUp.GetComponent<IngredientScript>().Igr.IsPick)
                {
                    _onHand = pickUp.GetComponent<IngredientScript>().Igr;
                    _onHand.IsPick = true;
                    pickUp.transform.parent = transform;
                    pickUp.transform.position = transform.position + transform.up + transform.right * facing/2;
                    pickUp.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }
        }
        else
        {
            if (currentWorkStation == null)
            {
                _onHand.IsPick = false;
                pickUp.transform.parent = null;
                pickUp.GetComponent<Rigidbody2D>().isKinematic = false;
                pickUp = null;
                _onHand = null;
            }
            else
            {
                FillWorkStation();
            }
        }
    }

    public void FillWorkStation ()
    {
        if (!currentWorkStation.WrkS.IsLoaded)
        {
            if (_onHand != null)
            {
                _onHand.IsPick = false;
                currentWorkStation.WrkS.CurrentIngredient.Add(_onHand);
                if (currentWorkStation.WrkS.CurrentIngredient.Count >= currentWorkStation.WrkS.MaximumIngredient)
                {
                    currentWorkStation.WrkS.IsLoaded = true;
                }
                Destroy(pickUp);
                _onHand = null;
            }
        }
        else
        {
            Debug.Log("Station is Full");
        }
    }

    public void ActivateWorkStation()
    {
        if (currentWorkStation != null && _onHand == null)
        {
            if (currentWorkStation.WrkS.IsLoaded)
            {
                bool correctIgr = IngredientCheck(currentWorkStation.WrkS.CurrentIngredient, currentWorkStation.CurrentCookingRecipe);
                if (correctIgr)
                {
                    Debug.Log("proceed to cooking");
                    switch (currentWorkStation.WrkS.Type)
                    {
                        case WorkStationType.SoupBoiler:
                            currentWorkStation.gameObject.GetComponent<BoilerScript>().toBoil = true;
                            break;
                    }
                }
                else
                {
                    Debug.Log("Wrong Ingredient");
                }

            }
        }
    }

    public bool IngredientCheck (List<Ingredient> igrCurrent, CookingRecipe recipeCurrent)
    {
        foreach(Ingredient r in recipeCurrent.IngredientLists)
        {
            if (!ContainsIngredient(igrCurrent, r))
            {
                return false;
            }
        }
        return true;
    }

    public bool ContainsIngredient (List<Ingredient> list, Ingredient igr)
    {
        foreach (Ingredient i in list)
        {
            if (i.Type == igr.Type)
                return true;
        }
        return false;
    }

}

//public enum PlayerState {Free, Chat, Working}