using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker2D : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 direction;
    private Camera mainCamera;
    private float objectWidth, objectHeight;

    void Start()
    {
        mainCamera = Camera.main;
        direction = GetRandomDirection();

        // 获取物体大小
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        objectWidth = sr.bounds.extents.x;
        objectHeight = sr.bounds.extents.y;
    }


    void FixedUpdate()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Vector2 pos = transform.position;
        Vector2 viewPos = mainCamera.WorldToViewportPoint(pos);

        // 水平方向反弹
        if (viewPos.x < 0 + (objectWidth / mainCamera.orthographicSize) || viewPos.x > 1 - (objectWidth / mainCamera.orthographicSize))
        {
            direction.x = -direction.x;
        }

        // 垂直方向反弹
        if (viewPos.y < 0 + (objectHeight / mainCamera.orthographicSize) || viewPos.y > 1 - (objectHeight / mainCamera.orthographicSize))
        {
            direction.y = -direction.y;
        }
    }

    Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    public void ChangeSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
