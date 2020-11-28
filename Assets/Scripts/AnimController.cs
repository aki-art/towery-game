using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Transform sprite;
    
    private void LateUpdate()
    {
        //sprite.position = Game.Instance.Tower.TranslatePosition(transform.position);
    }
}
