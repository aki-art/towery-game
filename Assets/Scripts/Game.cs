using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private Tower tower;
    public static Game Instance { get; private set; }
    public bool TransformTower = true;

    public Tower Tower => tower;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tower.Build();
    }
    
    void Update()
    {

    }
}