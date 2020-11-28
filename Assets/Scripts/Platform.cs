using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private PlatformDef def;

    private void Start()
    {
        // y this break the sprite completely tho
        //GetComponent<AnimController>().Sprite.color = def.color;

    }
}