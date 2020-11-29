using UnityEditor;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private PlatformDef def;

    private void Start()
    {
        // y this break the sprite completely tho
        //GetComponent<AnimController>().Sprite.color = def.color;

    }


    [DrawGizmo(GizmoType.NonSelected)]
    void DrawTriggerZone()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, collider.size);
    }
}