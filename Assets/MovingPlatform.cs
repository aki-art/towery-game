using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float ymin;
    float ymax;
    public float speed = 0.5f;
    public float height = 3;
    bool ascending = true;

    void Start()
    {
        ymin = transform.position.y;
        ymax = transform.position.y + height;
    }
    
    void FixedUpdate()
    {
        float move = Time.deltaTime * speed;

        if(ascending && transform.position.y > ymax)
            ascending = false;

        if(!ascending && ymin > transform.position.y)
            ascending = true;

        if (!ascending)
            move *= -1f;

        transform.position = new Vector2(transform.position.x, transform.position.y + move);

    }
}
