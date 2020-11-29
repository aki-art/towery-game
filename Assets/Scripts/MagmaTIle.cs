using UnityEngine;

public class MagmaTIle : MonoBehaviour
{
    public float damage = 1;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.relativeVelocity.x != 0 && collision.gameObject.TryGetComponent(out Health health))
        {
            health.Damage(damage);
        }
    }
}