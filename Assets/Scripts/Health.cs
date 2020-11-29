using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent OnTakeDamage;
    [SerializeField] float iframes = 2;

    public float maxHealth = 3;
    public float HP { get; private set; }

    float timeSinceLastHit = 0;

    TextMesh debugHPLabel;

    private void Awake()
    {
        HP = maxHealth;
        debugHPLabel = DebugUtil.DrawText(GetComponent<AnimController>().sprite.position + Vector3.up, $"{HP}/{maxHealth}");
        debugHPLabel.transform.SetParent(GetComponent<AnimController>().sprite);
    }

    private void Update()
    {
        timeSinceLastHit += Time.deltaTime;
    }

    bool CanTakeDamage() => timeSinceLastHit > iframes;

    public void Set(float value)
    {
        HP = Mathf.Clamp(value, 0, maxHealth);
        debugHPLabel.text = HP.ToString();
    }

    public void Damage(float damage, bool force = false)
    {
        if(CanTakeDamage())
        {
            Set(HP - damage);
            OnTakeDamage.Invoke();
            
            if (HP <= 0)
                OnDeath.Invoke();

            timeSinceLastHit = 0;
        }
    }
    
    public void Heal(float amount)
    {
        Set(HP + amount);
    }
}