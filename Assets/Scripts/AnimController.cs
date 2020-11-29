using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] public Transform sprite;
    private void LateUpdate() => sprite.position = transform.position.Transpose();
}
