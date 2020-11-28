using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float Speed = 7f;
    [SerializeField] private float Jumps = 8;
    [SerializeField] private float JumpSpeed = 7f;
    [SerializeField] private float GroundCheckDistance = 0.16f;
    [SerializeField] private float LedgeClimbHeight = 0.45f;
    [SerializeField] private float LedgeClimbSpeed = 0.3f;
    [SerializeField] private float LedgeClimbCheckDistance = 0.1f;
    [SerializeField] private float CollisionDistance = 0.1f;

    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private Collider2D mainCollider;
    

    float jumpCounter = 0;
    bool isGrounded = false;
    bool isJumping = false;
    bool stoppedJumping = false;
    int layerMask;

    PlayerStates playerState;
    PlayerStates previousState;

    bool facingLeft;

    Rigidbody2D rigidBody;
    Vector2 velocity;
    Vector2 currentVelocity;

    ContactFilter2D contactFilter;
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

    public PlayerController2 Instance { get; set; }

    void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        SetState(PlayerStates.Idle);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        currentVelocity = rigidBody.velocity;

        float x = Input.GetAxis("Horizontal");
        Direction direction = GetHorizontalDirection(x);

        Move(x);

        if (Mathf.Approximately(rigidBody.velocity.magnitude, 0))
            SetState(PlayerStates.Idle);

        if (playerState == PlayerStates.Running)
        {
            ClimbStairs(direction);
        }

        if (x != 0 && IsMovingLeft(x) != facingLeft)
            Flip();

        if (Input.GetButton("Jump"))
        {
            isJumping = true;
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            stoppedJumping = true;
        }
    }

    private void ClimbStairs(Direction direction)
    {
        float y = CanClimbStairs(direction) ? LedgeClimbSpeed : 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)ObjectLayers.Platform && IsGrounded())
        {
            if (!isGrounded)
            {
                OnLanded();
            }

            jumpCounter = 0;
            isGrounded = true;
            stoppedJumping = false;

        }
    }

    private void Move(float movement)
    {
        rigidBody.velocity = new Vector2(movement * Speed, rigidBody.velocity.y);

        if (isGrounded)
        {
            SetState(PlayerStates.Running);
        }
    }

    void Jump()
    {
        if (CanJump())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
            jumpCounter++;
            isGrounded = false;
            SetState(PlayerStates.Jumping);
        }
        else
        {
            stoppedJumping = true;
            jumpCounter = 0;
        }

        isJumping = false;
    }

    void OnLanded()
    {
        SetState(PlayerStates.Idle);
    }

    bool IsGrounded()
    {
        int count = groundCheck.Cast(Vector2.down, contactFilter, raycastHits, GroundCheckDistance);
        return count > 0;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
    
    bool IsMovingLeft(float input) => input < 0;

    public void SetState(PlayerStates state)
    {
        previousState = playerState;
        playerState = state;
    }

    Direction GetHorizontalDirection(float movement)
    {
        return IsMovingLeft(movement) ? Direction.Left : Direction.Right;
    }

    bool IsBottomObstructed(Direction direction)
    {
        float bottom = mainCollider.bounds.min.y + 0.01f;
        float left = mainCollider.bounds.min.x;
        float right = mainCollider.bounds.max.x;

        Vector2 bottomPoint;
        Vector2 checkDirection;


        if (direction == Direction.Left)
        {
            bottomPoint = new Vector2(left, bottom);
            checkDirection = Vector2.left;
        }
        else
        {
            bottomPoint = new Vector2(right, bottom);
            checkDirection = Vector2.right;
        }
        
        var bottomHit = Physics2D.Raycast(bottomPoint, checkDirection, CollisionDistance, layerMask);
        bool result = bottomHit.collider != null;

        Debug.DrawLine(bottomPoint, bottomPoint + checkDirection * CollisionDistance, result ? Color.red : Color.green, 0.15f);
        return result;
    }

    bool CanClimbStairs(Direction direction)
    {
        if (!IsBottomObstructed(direction))
            return false;

        float top = mainCollider.bounds.max.y + LedgeClimbHeight;
        float bottom = mainCollider.bounds.min.y + LedgeClimbHeight;
        float left = mainCollider.bounds.min.x;
        float right = mainCollider.bounds.max.x;

        Vector2 topPoint;
        Vector2 bottomPoint;
        Vector2 checkDirection;

        if (direction == Direction.Left)
        {
            topPoint = new Vector2(left, top);
            bottomPoint = new Vector2(left, bottom);
            checkDirection = Vector2.left;
        }
        else
        {
            topPoint = new Vector2(right, top);
            bottomPoint = new Vector2(right, bottom);
            checkDirection = Vector2.right;
        }

        var topHit = Physics2D.Raycast(topPoint, checkDirection, LedgeClimbCheckDistance);
        var bottomHit = Physics2D.Raycast(bottomPoint, checkDirection, LedgeClimbCheckDistance, (int)ObjectLayers.Platform);
        
        bool result = topHit.collider == null && bottomHit.collider == null;
        Color color = Color.white;
        if (topHit.collider != null)
            color = Color.cyan;
        if (bottomHit.collider != null)
            color = Color.blue;
        if (topHit.collider != null && bottomHit.collider != null)
            color = Color.red;
        Debug.DrawLine(bottomPoint + checkDirection * LedgeClimbCheckDistance, topPoint + checkDirection * LedgeClimbCheckDistance, color, 0.05f);
        Debug.Log(result);

        return result;
    }

    bool CanJump()
    {
        return (jumpCounter < Jumps || IsGrounded()) && !stoppedJumping;
    }

    enum Direction
    {
        Right,
        Left,
        Up,
        Down,
        None
    }
}
