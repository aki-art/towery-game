using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private float range;
    [SerializeField] private float offset = 0;
    [SerializeField] private float threshold = 0.01f;

    private void Start()
    {
        range = Game.Instance.Tower.Circumference / 2f;
    }

    void Update()
    {
        if (transform.position.x > range + offset - threshold)
            transform.position = new Vector3(-range + offset + threshold, transform.position.y, transform.position.z);
        else if (transform.position.x < -range + offset + threshold)
            transform.position = new Vector3(range + offset - threshold, transform.position.y, transform.position.z);
    }
}
