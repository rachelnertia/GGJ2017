using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public float halfWidthOfSprite = 23.52f / 2.0f;

    void Start()
    {

    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y);

        if (transform.position.x < -halfWidthOfSprite && moveSpeed < 0)
        {
            transform.position = new Vector2(halfWidthOfSprite, transform.position.y);
        }
        else if (transform.position.x > halfWidthOfSprite && moveSpeed > 0)
        {
            transform.position = new Vector2(-halfWidthOfSprite     , transform.position.y);
        }
    }
}