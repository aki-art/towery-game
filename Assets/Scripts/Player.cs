
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health health;

    public Health HP => health;

    public Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
}
