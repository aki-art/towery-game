using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private TowerDef def;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Transform followTarget;

    private GameObject towerBody;
    private float cellWidth;
    private float cellHeight;
    private float width;

    public float Circumference => def.circumference;
    
    public void Build()
    {
        cellWidth = Mathf.Pow(radius, 2) * Mathf.PI / def.circumference;
        cellHeight = def.cellHeight;

        width = def.circumference * cellWidth;

        towerBody = Instantiate(def.prefab, transform);
        towerBody.SetActive(true);
    }

    public Vector3 TranslatePosition(Vector3 pos)
    {
        if (Game.Instance.TransformTower)
        {
            float place = Mathf.PI * 2 * ((followTarget.position.x - pos.x) * cellWidth / width);
            return new Vector3(Mathf.Sin(place) * radius, pos.y, Mathf.Cos(place) * radius);
        }
        else return pos;
    }
    
}

