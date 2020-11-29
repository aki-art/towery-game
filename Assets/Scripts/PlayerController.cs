using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float Speed = 7f;
    [SerializeField] float Jumps = 8;
    [SerializeField] float JumpSpeed = 7f;
    [SerializeField] float LedgeClimbSpeed = 0.3f;
    [SerializeField] PlayerCollisions collisions;

    float jumpCounter = 0;
    bool isGrounded = false;
    bool stoppedJumping = false;
    bool facingLeft;

    TextMesh debug;

    Rigidbody2D rigidBody;

    public PlayerController Instance { get; set; }

    void OnEnable() => rigidBody = GetComponent<Rigidbody2D>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        debug = DebugUtil.DrawText(transform.position, "", Color.red);
        debug.transform.SetParent(transform);

        isGrounded = collisions.CheckGrounded();
    }

    // TODO: move some of this to fixed update
    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");

        Move(horizontalMovement);

        if (collisions.CheckGrounded())
        {
            if (!isGrounded)
                Land();
        }
        else isGrounded = false;

        if (Input.GetButton("Jump"))
            Jump();

        Flip(horizontalMovement);
    }

    private void Move(float movement)
    {
        rigidBody.velocity = new Vector2(movement * Speed, rigidBody.velocity.y);
        if (movement != 0 && CanClimb(movement))
            ClimbStairs(movement);
    }

    void Jump()
    {
        if (CanJump())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
            jumpCounter++;
        }
        else
            stoppedJumping = true;
    }
    
    void ClimbStairs(float movement)
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + LedgeClimbSpeed);
    }
    
    void Land()
    {
        jumpCounter = 0;
        stoppedJumping = false;
        isGrounded = true;
    }

    void Flip(float x)
    {
        if (x != 0 && x < 0 != facingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            GetComponent<AnimController>().transform.localScale = scale;
            facingLeft = !facingLeft;
        }
    }

    bool CanJump() => (jumpCounter < Jumps || isGrounded) && !stoppedJumping;
    bool CanClimb(float dir) => collisions.CanClimbStairs(dir);
}
