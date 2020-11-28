using System.Collections.Generic;
using UnityEngine;

// based on Unity Guide, not used atm
public class PhysicsObject : MonoBehaviour
{
    protected const float MIN_MOVE_DISTANCE = 0.001f;
    protected const float SHELL_RADIUS = 0.01f;

    public float gravityModifier = 1f;
    public float minGroundNormalY = 0.65f;

    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 velocity;
    protected Vector2 targetVelocity;
    protected Rigidbody2D rigidBody;
    protected ContactFilter2D contactFilter;
    protected List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

    void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 movement = moveAlongGround * deltaPosition.x;
        Move(movement, false);

        movement = Vector2.up * deltaPosition.y;
        Move(movement, true);
    }

    private void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    private void Move(Vector2 movement, bool YMovement)
    {
        float distance = movement.magnitude;

        if(distance > MIN_MOVE_DISTANCE)
        {
            int count = rigidBody.Cast(movement, contactFilter, raycastHits, distance + SHELL_RADIUS);

            foreach (var hit in raycastHits)
            {
                Vector2 currentNormal = hit.normal;
                if(currentNormal.y > minGroundNormalY)
                {
                    grounded = true;

                    if (YMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity -= projection * currentNormal;
                }

                float modifiedDistance = hit.distance - SHELL_RADIUS;
                distance = Mathf.Min(modifiedDistance, distance);

            }
        }

        rigidBody.position += movement.normalized * distance;
    }
}
