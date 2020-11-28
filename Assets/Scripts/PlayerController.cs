using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on Unity Guide, not used atm
public class  PlayerController : PhysicsObject
{
    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    public float deceleration = 0.5f;
    Collider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            boxCollider.enabled = false;
        }
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0)
            {
                velocity.y *= deceleration;
            }
        }

        targetVelocity = move * maxSpeed;
    }
}