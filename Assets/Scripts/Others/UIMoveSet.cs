using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveSet : MonoBehaviour
{
    [Header("摆动参数")]
    public float swingSpeed = 2f;      // 摆动速度（频率）
    public float swingAmount = 50f;    // 摆动幅度（像素）
    public Vector2 swingDirection = Vector2.right; // 摆动方向（默认水平）

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private float timer = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void FixedUpdate()
    {
        // 计时器增长（使用固定时间步长）
        timer += Time.fixedDeltaTime;

        // 计算正弦值（范围[-1,1]），并映射到摆动幅度
        float swingValue = Mathf.Sin(timer * swingSpeed) * swingAmount;

        // 计算新位置（保持Y轴不变，仅水平摆动）
        Vector2 newPosition = startPosition + swingDirection * swingValue;

        // 应用新位置
        rectTransform.anchoredPosition = newPosition;
    }

    // 重置到起始位置（可选）
    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        timer = 0f;
    }
}
