using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerCollisions collisions;
    Health health;
    public float crushDamage = 1;
    public float colliderTurnOffSeconds = 0.5f;

    private void Start()
    {
        collisions = GetComponent<PlayerCollisions>();
        health = GetComponent<Health>();
        health.OnDeath.AddListener(OnDeath);
        health.OnTakeDamage.AddListener(OnTakeDamage);
    }

    private void OnTakeDamage()
    {
        collisions.Collider.enabled = false;
        StartCoroutine(ResumeCollider());
    }

    IEnumerator ResumeCollider()
    {
        yield return new WaitForSeconds(colliderTurnOffSeconds);
        collisions.Collider.enabled = true;
    }

    private void OnDeath()
    {
        Debug.Log("Oh no! we died.");
    }

    private void Update()
    {
        if(collisions.IsBeingCrushed())
        {
            health.Damage(1f);
        }
    }
}
