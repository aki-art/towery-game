using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private Collider2D mainCollider;
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private Collider2D stairsCheck;

    [SerializeField] private float GroundCheckDistance = 0.1f;
    [SerializeField] private float LedgeClimbHeight = 0.45f;
    [SerializeField] private float LedgeClimbCheckDistance = 0.51f;
    [SerializeField] private float CeilingCrushInset = 0.05f;

    ContactFilter2D contactFilter;
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    List<RaycastHit2D> stairCheckHits = new List<RaycastHit2D>();

    public Collider2D Collider => mainCollider;

    int layerMask;

    private void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
    }
    
    public bool IsBeingCrushed()
    {
        return CheckGrounded() && CheckCeiling(CeilingCrushInset);
    }

    private bool CheckCeiling(float offset)
    {
        return groundCheck.Cast(Vector2.up, contactFilter, raycastHits, GroundCheckDistance + mainCollider.bounds.size.y - offset) > 0;
    }

    public bool CheckGrounded()
    {
        return groundCheck.Cast(Vector2.down, contactFilter, raycastHits, GroundCheckDistance) > 0;
    }
    
    public bool BottomBumping(float movement)
    {
        var checkDirection = new Vector2(movement, GroundCheckDistance);
        return groundCheck.Cast(checkDirection, contactFilter, raycastHits, GroundCheckDistance) > 0;
    }

    public bool CanClimbStairs(float movement)
    {
        if (movement == 0 || !BottomBumping(movement))
            return false;

        Vector2 dir = movement < 0 ? Vector2.left : Vector2.right;
        return stairsCheck.Cast(dir, contactFilter, stairCheckHits, LedgeClimbCheckDistance) == 0;
    }
}
